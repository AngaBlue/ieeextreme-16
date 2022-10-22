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

# Get conditions
n = get_number()
m = get_number()
p = get_number()

# Define a 0 array of length n
array = [0] * n

# Loop m times
for i in range(m):
    try:
        # Get number of tests and loop
        l = get_number()
        r = get_number()
        k = get_number()

    except EOFError:
        break
