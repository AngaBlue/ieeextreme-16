import numpy as np

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

# Get number of tests
tests = get_number()

for t in range(tests):
    # Create array with dimensions
    n = get_number() # lines per level
    m = get_number() # characters per line
    d = get_number() # levels

    array = np.empty(shape=[d, n, m], dtype='S1')

    # Populate array
    for i in range(d):
        for j in range(n):
            word = get_word()
            while (len(word) < m):
                word = get_word()
                
            for k in range(m):
                array[i, j, k] = word[k]

    # Print array
    # print(array)

    # Count
    inputs = np.count_nonzero(array[0,:,:] == b'o')
    outputs = np.count_nonzero(array[d-1,:,:] == b'o')
    pipes = np.count_nonzero(array == b'*')

    internal_connections = (inputs + outputs + pipes * 3) - inputs - outputs
    print('YES' if internal_connections % 2 == 0 else 'NO')
    
