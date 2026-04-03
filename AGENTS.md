# BetterUI Repo Instructions

- When we're developing mods in this repo, Codex owns all CLI work.
- Codex should run git, build, test, packaging, publish, and other shell commands directly unless blocked by missing auth or an external UI-only step.
- Keep user-facing instructions focused on decisions, validation, and non-CLI actions.
- Do not version-bump for debugging passes or UI tuning such as font, spacing, alignment, or offset tweaks; republish in place unless the user explicitly wants a release/version change.
