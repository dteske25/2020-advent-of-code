with open('R:\\Code\\2020-advent-of-code\\3-toboggan-trajectory\\data.txt') as f:
    content = [line.rstrip() for line in f]
    trees1_1 = 0
    trees1_3 = 0
    trees1_5 = 0
    trees1_7 = 0
    trees2_1 = 0
    for rowIndex in range(len(content)):
        if content[rowIndex][rowIndex % len(content[rowIndex])] == '#':
            trees1_1 += 1
        if content[rowIndex][(rowIndex * 3) % len(content[rowIndex])] == '#':
            trees1_3 += 1
        if content[rowIndex][(rowIndex * 5) % len(content[rowIndex])] == '#':
            trees1_5 += 1
        if content[rowIndex][(rowIndex * 7) % len(content[rowIndex])] == '#':
            trees1_7 += 1
        if (rowIndex % 2 == 0) and content[rowIndex][(rowIndex // 2) % len(content[rowIndex])] == '#':
            trees2_1 += 1
    print(f'Trees1_1: {trees1_1}')
    print(f'Trees1_3: {trees1_3}')
    print(f'Trees1_5: {trees1_5}')
    print(f'Trees1_7: {trees1_7}')
    print(f'Trees2_1: {trees2_1}')
    print(f'Total: {trees1_1 * trees1_3 * trees1_5 * trees1_7 * trees2_1}')

