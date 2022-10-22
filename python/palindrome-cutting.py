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

N = get_number()
M = get_number()
