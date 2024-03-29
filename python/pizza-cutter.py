# 100% Completed

from typing import Set

# Boilerplate parsing
def parser():
    while 1:
        try:
            data = list(input().split(' '))
            for number in data:
                if len(number) > 0:
                    yield(number)   
        except EOFError:
            return

input_parser = parser()

def get_word():
    global input_parser
    return next(input_parser)

def get_number():
    data = get_word()
    try:
        return int(data)
    except ValueError:
        return float(data)

# Get number of tests and loop
tests = get_number()
for test in range(tests):
    # Sets won't allow duplicates
    angles: Set[int] = set()

    # Get number of values in test case
    test_length = get_number()
    for test in range(test_length):
        angle = get_number()
        angles.add((angle % 180 + 180) % 180)

    # Number of slices = number of unique angles * 2
    slices = len(angles) * 2
    print(slices if slices else 1)
