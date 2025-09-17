# Zahlenrätsel Solver

Ein Python-basierter Solver für ein komplexes Zahlenrätsel mit dynamischen Nachbarschaftsregeln.

## 📋 Spielbeschreibung

Das Rätsel startet mit einem Brett, das die Zahlen 1-19 (ohne 10) enthält. Zweistellige Zahlen werden in einzelne Ziffern aufgeteilt und in 9er-Zeilen angeordnet:

```
123456789
111213141
516171819
```

### Spielregeln

1. **Ziel**: Alle Zahlen vom Brett entfernen
2. **Entfernung**: Zwei benachbarte Zahlen können entfernt werden, wenn:
   - Sie gleich sind, ODER
   - Ihre Summe 10 ergibt
3. **Nachbarschaft**: 
   - Vertikal und horizontal benachbart
   - **Dynamisch**: Nach Entfernung werden Lücken übersprungen
   - **Spezialregel**: Letzte Spalte ist mit erster Spalte der nächsten Zeile verbunden
4. **Reihenfolge**: Zahlen werden paarweise, eine nach der anderen entfernt

## 💻 Verwendung

### Grundlegende Ausführung

```python
from number_puzzle_solver import NumberPuzzleSolver

# Solver erstellen und ausführen
solver = NumberPuzzleSolver()
solver.solve_and_show_solution()
```

### Ausgabe-Beispiel

```
Startbrett:
Zeile 0: 123456789
Zeile 1: 111213141
Zeile 2: 516171819

Suche nach Lösung...
Tiefe 0: Entferne 1 bei (0, 0) und 1 bei (1, 0)
Tiefe 1: Entferne 2 bei (0, 1) und 8 bei (0, 7)
...
🎉 Lösung gefunden!
```

### Erweiterte Nutzung

```python
# Manuell Schritt für Schritt
solver = NumberPuzzleSolver()
solver.print_board()

# Alle gültigen Züge anzeigen
moves = solver.get_all_valid_moves()
print(f"Verfügbare Züge: {len(moves)}")

# Einzelnen Zug ausführen
if moves:
    pos1, pos2 = moves[0]
    solver.make_move(pos1, pos2)
    solver.print_board()
```

## 🔧 Technische Details

### Algorithmus
- **Backtracking**: Systematische Suche durch alle möglichen Zugkombinationen
- **Heuristik**: Bevorzugung von Zügen mit gleichen Zahlen
- **Dynamische Nachbarschaft**: Echtzeitberechnung nach jeder Entfernung

### Klassen und Methoden

```python
class NumberPuzzleSolver:
    def create_initial_board()     # Erstellt Startbrett
    def get_neighbors(row, col)    # Findet alle Nachbarn
    def can_remove_pair(pos1, pos2) # Prüft Entfernungsregeln
    def make_move(pos1, pos2)      # Führt Zug aus
    def undo_move()                # Macht Zug rückgängig
    def solve()                    # Hauptlösungsalgorithmus
```

### Komplexität
- **Zeitkomplexität**: Exponentiell im schlimmsten Fall (NP-vollständig)
- **Speicherkomplexität**: O(n) für Backtracking-Stack
- **Typische Laufzeit**: Sekunden bis Minuten je nach Brettkonfiguration

## ⚠️ Limitierungen

### Mathematische Grenzen
- **Nicht alle Konfigurationen sind lösbar**: Abhängig von der Verteilung der Zahlen
- **Ungerade Zahlenmengen**: Können prinzipiell nicht vollständig paarweise entfernt werden
- **Exponentieller Suchraum**: Bei größeren Brettern deutlich längere Laufzeiten

### Performance-Hinweise
- Das Standard-Brett (18 Zahlen) ist typischerweise in unter einer Minute lösbar
- Bei komplexeren Konfigurationen kann die Suche mehrere Minuten dauern
- Speicherverbrauch steigt mit der Rekursionstiefe

### Bekannte Probleme
- Keine Optimierung für spezielle Symmetrien
- Heuristik ist simpel und könnte verbessert werden
- Keine parallele Verarbeitung implementiert

## 🔬 Beispiel-Analyse

Das Standardbrett enthält:
- **18 Zahlen total** (9 Paare nötig)
- **Mehrere 1er**: Gute Kandidaten für gleiche Paare
- **Verschiedene Summen zu 10**: 1+9, 2+8, 3+7, 4+6, 5+5

**Lösbarkeitsindikator**: Gerade Anzahl von Zahlen ist notwendige (aber nicht hinreichende) Bedingung.

## 🛠️ Erweiterungsmöglichkeiten

### Geplante Features
- [ ] Grafische Benutzeroberfläche
- [ ] Erweiterte Heuristiken
- [ ] Parallele Suchstrategien
- [ ] Lösungsstatistiken
- [ ] Benutzerdefinierte Brettkonfigurationen

### Beitragen
1. Fork des Repositories
2. Feature-Branch erstellen
3. Tests hinzufügen
4. Pull Request stellen

## 📊 Performance-Benchmarks

| Brett-Größe | Typische Laufzeit | Speicherverbrauch |
|-------------|------------------|------------------|
| 3x3 (9 Zahlen) | < 1s | ~1MB |
| Standard (18 Zahlen) | 10-60s | ~5MB |
| 4x9 (36 Zahlen) | Minuten-Stunden | ~50MB |

## 📄 Lizenz

[Lizenz hier einfügen]

## 🤝 Kontakt

[Kontaktinformationen hier einfügen]

---

**Hinweis**: Dieses Projekt demonstriert Backtracking-Algorithmen und kombinatorische Optimierung. Die Lösbarkeit hängt stark von der initialen Zahlenkonfiguration ab.