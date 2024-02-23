When moving a piece pls remember that there are 2 different storage systems and after every change both !!!
    1.  Dictionary _piecePosition
    2.  Tile.piece

// Check how many lines of code
Get-ChildItem -Recurse -Filter '*.cs' | ForEach-Object { Get-Content $_.FullName | Measure-Object -Line | Select-Object Lines, Path } | Measure-Object -Property Lines -Sum