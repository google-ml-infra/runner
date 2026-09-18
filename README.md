This is not an officially supported Google product. This project is not
eligible for the [Google Open Source Software Vulnerability Rewards
Program](https://bughunters.google.com/open-source-security).

# Google ML Infrastructure Actions Runner

This repository is a **hard fork** of [github.com/actions/runner](https://github.com/actions/runner). It is customized and maintained by the Google ML Infrastructure team to support the ML Actions Platform.

## Why a Hard Fork?

Standard GitHub Actions self-hosted runners rely on local filesystems or shared network volumes to share directories (like the workspace) between the runner process and job containers. In a Kubernetes environment (like GKE) at scale, sharing volumes introduces storage latency, and executing commands via `kubectl exec` is prone to network fragility.

To address these challenges, this fork implements a **serverless execution model** specifically optimized for GKE:
- **No Shared Volumes**: The runner dynamically schedules job pods without requiring shared network storage (e.g. NFS/Filestore or Persistent Disks).
- **Direct Agent Orchestration**: A lightweight agent is injected into job containers to handle execution and file management over the GKE pod-to-pod network.
- **Native Kubernetes Management**: The runner orchestrates pod lifecycles directly using the Kubernetes API instead of relying on legacy external hooks.

## Roadmap & Repository Strategy

This repository will act as the single source of truth for the custom runner binary used across the ML Actions Platform. 

Our plan for this repository includes:
1. **Upstream Merges**: Periodically sync with upstream releases of `actions/runner` to pull in security patches, runner updates, and new features, while preserving our GKE-native enhancements.
2. **Kubernetes Native Orchestration**: Continued hardening of the C#-native `KubernetesManager` to directly control job pod setup, security constraints, and automatic resource cleanup.
3. **Execution Agent Optimizations**: Further enhancements to execution reliability, gRPC streaming protocols, and bidirectional file syncing inside job containers.
4. **mTLS & Security Controls**: Restricting control-plane communications via mutual TLS and scoped authentication tokens to ensure secure tenant isolation in shared GKE namespaces.
