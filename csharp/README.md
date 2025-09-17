# ZahlenStreichen

## 📖 Overview

This project implements a solver for the "Zahlen Streich" puzzle in C#. The puzzle involves removing numbers from a board based on specific rules, with the goal of clearing the board entirely. The solution is implemented using LINQ and parallel processing for performance optimization.

---

## 🧩 Puzzle Rules

1. **Initial Board**:
   - The numbers 1 to 19 (excluding 10) are arranged in rows of 9 columns.
   - Two-digit numbers are split into individual digits.
   - Example starting board:
     ```
     123456789
     111213141
     516171819
     ```

2. **Removal Rules**:
   - Two numbers can be removed if:
     - They are direct neighbors (up, down, left, or right).
     - Their values are equal, OR their sum equals 10.
   - Neighbors skip over solved cells dynamically.
   - Special rules:
     - The last column of a row connects to the first column of the next row.
     - The first column of a row connects to the last column of the previous row.

3. **Goal**:
   - Remove all numbers from the board.

---

## 🛠️ Project Structure

### Key Files

- **[`Program.cs`](ZahlenStreichen/Program.cs)**: Entry point of the application. Implements the main loop for solving the puzzle.
- **[`Number.cs`](ZahlenStreichen/Number.cs)**: Represents individual numbers on the board, including their value, position, and relationships with neighbors.
- **[`NumberBoard.cs`](ZahlenStreichen/NumberBoard.cs)**: Manages the board state, including the list of numbers, possible moves, and solution markers.
- **[`SolutionMarker.cs`](ZahlenStreichen/SolutionMarker.cs)**: Tracks pairs of numbers that have been solved and the solution type.
- **[`Solutions.cs`](ZahlenStreichen/Solutions.cs)**: Defines the possible solution types as an enum (e.g., `EqualTop`, `TenNext`).
- **[`EnumExtensions.cs`](ZahlenStreichen/EnumExtensions.cs)**: Provides utility methods for working with enums, such as extracting individual flags.

### Solution File

- **[`ZahlenStreichen.sln`](ZahlenStreichen.sln)**: Visual Studio solution file for the project.

---

## 🚀 How to Run

### Prerequisites

- **.NET Framework 4.5** or higher.
- Visual Studio 2013 or later.

### Steps

1. Open the solution file `ZahlenStreichen.sln` in Visual Studio.
2. Build the solution to restore dependencies and compile the code.
3. Run the project in Debug or Release mode.

---

## 🧩 How It Works

1. **Initialization**:
   - The board is created with numbers 1-19 (excluding 10), split into rows of 9 columns.

2. **Solving**:
   - The program iteratively finds all possible moves (pairs of numbers that can be removed).
   - If no moves are possible, the board is extended dynamically.
   - The solution uses parallel processing (`AsParallel`) to speed up the search for solutions.

3. **Output**:
   - The program prints the board state and the solution steps to the console.

---

## 🛠️ Key Classes and Methods

### `NumberBoard`

- **`GetPossibleMarker()`**: Finds all valid moves on the current board.
- **`SetMarker(SolutionMarker marker)`**: Marks a pair of numbers as solved.
- **`PrintBoard()`**: Prints the current board state to the console.

### `Number`

- **`Solutions`**: Calculates the possible solutions for a number based on its neighbors.
- **`GetMarker()`**: Generates solution markers for valid moves involving the number.

### `SolutionMarker`

- **`IsNumberToMark(Number number)`**: Checks if a number is part of the solution.
- **`SetSolution(Number number)`**: Marks a number as solved with a specific solution type.

---

## 🧪 Example Output

```
123456789
111213141
516171819

1st run - 10 games in 100ms - Min. Solutions 5
2nd run - 8 games in 80ms - Min. Solutions 3
...
🎉 Solution found!
```

---

## 📄 License

This project is licensed under the [MIT License](../LICENSE).

---

## 🤝 Contributing

Contributions are welcome! Feel free to fork the repository, make changes, and submit a pull request.

---

## 📧 Contact

For questions or feedback, please contact the