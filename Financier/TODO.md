# Financier's TODO list
Here is a list of all current and past TODOs. This also serves as a development milestones history.

## Pages

- [x] **Home** (dashboard) page
	- [x] Redesign to modern card-based layout
	- [x] Integrated Balance display with Accent color
	- [ ] Logout button (return to profile selection)
	- [ ] Quick transaction history edits (Inline editing)

- [x] **New Transaction** page
	- [x] Transaction fields
	- [x] Separate income and expense transaction types
	- [x] Refactor to professional WinUI form
	- [x] Use NumberBox for better numeric input

- [x] **Settings** page
	- [x] Basic styling settings
	- [x] Refactor to grouped professional layout
	- [ ] More advanced settings (Currency selection, Export options)

- [x] **Profile switcher** page
	- [x] Profile items with custom template
	- [x] Refactor to modern list layout

- [x] **New profile** page
	- [x] Professional form redesign

## Controls
- [x] Better history view item (transaction item)
- [x] General transaction item instead of a simple TextBlock
	- [x] Add theme-aware colors to separate income/expenses
	- [x] Add icons to visually identify transaction type

## Logic
- [x] **User profile** IO (JSON based)
- [x] **Model Refactoring** (Switched from structs to classes for better performance/reliability)
- [ ] Implement proper `async Task` IO throughout the app
- [ ] Add "Delete Transaction" functionality
- [ ] Implement category-based filtering/search on Home page

## Miscellaneous
- [ ] Design app icons instead of the default ones
- [ ] Create the app certificate (local only)