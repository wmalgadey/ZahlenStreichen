class NumberPuzzleSolver:
    def __init__(self):
        # Erstelle das Startbrett: 1-19 ohne 10
        self.original_board = self.create_initial_board()
        self.board = [row[:] for row in self.original_board]  # Kopie für Manipulation
        self.rows = len(self.board)
        self.cols = len(self.board[0])
        self.moves = []  # Speichere alle Züge
        
    def create_initial_board(self):
        """Erstellt das Startbrett mit Zahlen 1-19 ohne 10"""
        numbers = [i for i in range(1, 20) if i != 10]  # 1-19 ohne 10
        
        # Konvertiere Zahlen zu einzelnen Ziffern
        digits = []
        for num in numbers:
            if num < 10:
                digits.append(num)
            else:
                # Zweistellige Zahlen: jede Ziffer einzeln
                digits.extend([int(d) for d in str(num)])
        
        # Verteile in 9er-Zeilen
        board = []
        for i in range(0, len(digits), 9):
            row = digits[i:i+9]
            if len(row) < 9:
                row.extend([None] * (9 - len(row)))  # Fülle mit None auf
            board.append(row)
            
        return board
    
    def print_board(self):
        """Zeigt das aktuelle Brett"""
        print("\nAktuelles Brett:")
        for i, row in enumerate(self.board):
            row_str = ""
            for j, val in enumerate(row):
                if val is None:
                    row_str += "."
                else:
                    row_str += str(val)
            print(f"Zeile {i}: {row_str}")
        print()
    
    def get_neighbors(self, row, col):
        """Ermittelt alle Nachbarn einer Position unter Berücksichtigung der dynamischen Regeln"""
        neighbors = []
        
        # Richtungen: oben, unten, links, rechts
        directions = [(-1, 0), (1, 0), (0, -1), (0, 1)]
        
        for dr, dc in directions:
            neighbor = self.find_neighbor_in_direction(row, col, dr, dc)
            if neighbor:
                neighbors.append(neighbor)
                
        # Spezialregel: Letzte Spalte -> erste Spalte nächste Zeile
        if col == self.cols - 1 and row < self.rows - 1:
            neighbor = self.find_neighbor_in_direction(row + 1, 0, 0, 0)
            if neighbor:
                neighbors.append(neighbor)
                
        # Spezialregel: Erste Spalte -> letzte Spalte vorherige Zeile
        if col == 0 and row > 0:
            neighbor = self.find_neighbor_in_direction(row - 1, self.cols - 1, 0, 0)
            if neighbor:
                neighbors.append(neighbor)
        
        return neighbors
    
    def find_neighbor_in_direction(self, row, col, dr, dc):
        """Findet den nächsten nicht-None Nachbarn in einer Richtung"""
        if dr == 0 and dc == 0:  # Startposition
            if (0 <= row < self.rows and 0 <= col < self.cols and 
                self.board[row][col] is not None):
                return (row, col)
            return None
            
        current_row, current_col = row + dr, col + dc
        
        while (0 <= current_row < self.rows and 0 <= current_col < self.cols):
            if self.board[current_row][current_col] is not None:
                return (current_row, current_col)
            current_row += dr
            current_col += dc
            
        return None
    
    def can_remove_pair(self, pos1, pos2):
        """Prüft, ob zwei Positionen entfernt werden können"""
        r1, c1 = pos1
        r2, c2 = pos2
        
        # Beide Positionen müssen gültige Zahlen haben
        if (self.board[r1][c1] is None or self.board[r2][c2] is None):
            return False
            
        val1, val2 = self.board[r1][c1], self.board[r2][c2]
        
        # Prüfe ob sie Nachbarn sind
        neighbors1 = self.get_neighbors(r1, c1)
        if pos2 not in neighbors1:
            return False
            
        # Prüfe Entfernungsregeln: gleich oder Summe = 10
        return val1 == val2 or val1 + val2 == 10
    
    def get_all_valid_moves(self):
        """Ermittelt alle gültigen Züge"""
        valid_moves = []
        positions = [(r, c) for r in range(self.rows) for c in range(self.cols) 
                    if self.board[r][c] is not None]
        
        for i, pos1 in enumerate(positions):
            for pos2 in positions[i+1:]:
                if self.can_remove_pair(pos1, pos2):
                    valid_moves.append((pos1, pos2))
                    
        return valid_moves
    
    def make_move(self, pos1, pos2):
        """Führt einen Zug aus"""
        r1, c1 = pos1
        r2, c2 = pos2
        
        # Speichere die entfernten Werte für Rückgängigmachen
        val1, val2 = self.board[r1][c1], self.board[r2][c2]
        
        # Entferne die Zahlen
        self.board[r1][c1] = None
        self.board[r2][c2] = None
        
        # Speichere den Zug
        self.moves.append((pos1, pos2, val1, val2))
        
        return val1, val2
    
    def undo_move(self):
        """Macht den letzten Zug rückgängig"""
        if not self.moves:
            return False
            
        pos1, pos2, val1, val2 = self.moves.pop()
        r1, c1 = pos1
        r2, c2 = pos2
        
        self.board[r1][c1] = val1
        self.board[r2][c2] = val2
        
        return True
    
    def is_solved(self):
        """Prüft, ob das Puzzle gelöst ist (alle Zahlen entfernt)"""
        for row in self.board:
            for val in row:
                if val is not None:
                    return False
        return True
    
    def solve(self, max_depth=None):
        """Löst das Puzzle mit Backtracking"""
        if max_depth is None:
            # Berechne maximale Tiefe basierend auf Anzahl der Zahlen
            total_numbers = sum(1 for row in self.board for val in row if val is not None)
            max_depth = total_numbers // 2
        
        return self.backtrack(0, max_depth)
    
    def backtrack(self, depth, max_depth):
        """Backtracking-Algorithmus"""
        if self.is_solved():
            return True
            
        if depth >= max_depth:
            return False
            
        valid_moves = self.get_all_valid_moves()
        
        if not valid_moves:
            return False
            
        # Sortiere Züge nach Heuristik (bevorzuge Züge mit gleichen Zahlen)
        valid_moves.sort(key=lambda move: (
            self.board[move[0][0]][move[0][1]] == self.board[move[1][0]][move[1][1]],
            -(self.board[move[0][0]][move[0][1]] + self.board[move[1][0]][move[1][1]])
        ), reverse=True)
        
        for pos1, pos2 in valid_moves:
            # Mache den Zug
            val1, val2 = self.make_move(pos1, pos2)
            
            print(f"Tiefe {depth}: Entferne {val1} bei {pos1} und {val2} bei {pos2}")
            
            # Rekursiver Aufruf
            if self.backtrack(depth + 1, max_depth):
                return True
                
            # Rückgängig machen
            self.undo_move()
            
        return False
    
    def solve_and_show_solution(self):
        """Löst das Puzzle und zeigt die Lösung"""
        print("Startbrett:")
        self.print_board()
        
        print("Suche nach Lösung...")
        if self.solve():
            print("\n🎉 Lösung gefunden!")
            print(f"Anzahl Züge: {len(self.moves)}")
            print("\nLösungsweg:")
            
            # Zeige Lösungsweg
            self.board = [row[:] for row in self.original_board]  # Reset
            for i, (pos1, pos2, val1, val2) in enumerate(self.moves):
                print(f"Zug {i+1}: Entferne {val1} bei {pos1} und {val2} bei {pos2}")
                self.board[pos1[0]][pos1[1]] = None
                self.board[pos2[0]][pos2[1]] = None
                
            print("\nEndstand:")
            self.print_board()
            
        else:
            print("❌ Keine Lösung gefunden!")
            
        return len(self.moves) > 0

def main():
    solver = NumberPuzzleSolver()
    solver.solve_and_show_solution()

if __name__ == "__main__":
    main()
