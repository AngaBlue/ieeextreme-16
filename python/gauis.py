alpha = ['a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z']
def Decrypt(Encryptedtext):
    solution = ""
    for i in range(len(Encryptedtext)):
        for j in range(len(alpha)):
            if Encryptedtext[i] == alpha[j]:
                solution = solution + alpha[j - 12]
                break
            elif Encryptedtext[i] == alpha[j].upper():
                solution = solution + alpha[j - 12].upper()
                break
            elif j == 25:
                solution = solution + Encryptedtext[i]
    return solution
print(Decrypt(input()))
