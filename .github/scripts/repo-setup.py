#!/usr/bin/env python3
"""
repo-setup.py

Script to help configure repository settings programmatically via the GitHub REST API.

Usage:
  python .github/scripts/repo-setup.py --owner <owner> --repo <repo> --token <GH_TOKEN>

This script will:
 - Apply branch protection rules defined in .github/branch-protection.yml
 - Create standard labels
 - Optionally enable repository features

Note: This script can't run without a personal access token with 'repo' scope.
"""

import argparse
import json
import os
import sys
import requests


def load_branch_protection(path):
    with open(path, 'r', encoding='utf-8') as f:
        return json.loads(json.dumps(__import__('yaml').safe_load(f)))


def apply_branch_protection(owner, repo, token, rules):
    headers = {
        'Authorization': f'token {token}',
        'Accept': 'application/vnd.github+json',
    }
    session = requests.Session()
    session.headers.update(headers)

    for rule in rules.get('rules', []):
        pattern = rule.get('pattern')
        if not pattern:
            continue
        url = f'https://api.github.com/repos/{owner}/{repo}/branches/{pattern}/protection'
        payload = {
            'required_status_checks': None,
            'enforce_admins': rule.get('enforce_admins', False),
            'required_pull_request_reviews': None,
            'restrictions': None,
            'allow_force_pushes': rule.get('allow_force_pushes', False),
            'allow_deletions': rule.get('allow_deletions', False),
            'required_linear_history': rule.get('required_linear_history', False),
        }

        # Required status checks
        rsc = rule.get('required_status_checks')
        if rsc and rsc.get('contexts'):
            payload['required_status_checks'] = {
                'strict': rsc.get('strict', False),
                'contexts': rsc.get('contexts', [])
            }

        # Pull request reviews
        if rule.get('required_approving_review_count', 0) > 0 or rule.get('require_code_owner_reviews'):
            payload['required_pull_request_reviews'] = {
                'required_approving_review_count': rule.get('required_approving_review_count', 1),
                'require_code_owner_reviews': rule.get('require_code_owner_reviews', False)
            }

        print(f'Applying branch protection for {pattern}...')
        resp = session.put(url, json=payload)
        if resp.status_code in (200, 201):
            print(f'  OK: {pattern}')
        else:
            print(f'  Failed ({resp.status_code}): {resp.text}')


def create_labels(owner, repo, token, labels):
    headers = {
        'Authorization': f'token {token}',
        'Accept': 'application/vnd.github+json',
    }
    session = requests.Session()
    session.headers.update(headers)
    url = f'https://api.github.com/repos/{owner}/{repo}/labels'

    for label in labels:
        payload = {'name': label['name'], 'color': label.get('color', 'ededed'), 'description': label.get('description','')}
        resp = session.post(url, json=payload)
        if resp.status_code in (200, 201):
            print(f"Created label: {label['name']}")
        elif resp.status_code == 422:
            print(f"Label exists: {label['name']}")
        else:
            print(f"Failed to create label {label['name']}: {resp.status_code} {resp.text}")


def main():
    parser = argparse.ArgumentParser()
    parser.add_argument('--owner', required=True)
    parser.add_argument('--repo', required=True)
    parser.add_argument('--token', required=True)
    args = parser.parse_args()

    here = os.path.dirname(os.path.abspath(__file__))
    bp_path = os.path.join(here, '..', 'branch-protection.yml')
    try:
        rules = load_branch_protection(bp_path)
    except Exception as e:
        print('Failed to load branch protection file:', e)
        sys.exit(1)

    apply_branch_protection(args.owner, args.repo, args.token, rules)

    labels = [
        {'name': 'bug', 'color': 'd73a4a', 'description': 'A bug in the project'},
        {'name': 'enhancement', 'color': 'a2eeef', 'description': 'New feature or request'},
        {'name': 'documentation', 'color': '0075ca', 'description': 'Documentation updates'},
        {'name': 'question', 'color': 'd876e3', 'description': 'Further information is requested'},
    ]
    create_labels(args.owner, args.repo, args.token, labels)

    print('\nRepository setup script finished. Review output for any failures.')


if __name__ == '__main__':
    main()
