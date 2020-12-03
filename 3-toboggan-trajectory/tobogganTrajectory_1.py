with open('R:\\Code\\2020-advent-of-code\\3-toboggan-trajectory\\data.txt') as f:
    content = [line.rstrip() for line in f]
    trees = 0
    for rowIndex in range(len(content)):
        posX = (rowIndex * 3) % len(content[rowIndex])
        if content[rowIndex][posX] == '#':
            trees += 1
    print(f'Trees: {trees}')

