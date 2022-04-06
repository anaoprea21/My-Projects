import threading
from random import randint
import Token
from Computer import Computer

def initializeToken(computers):
    list = computers.copy()
    message = "Hi there"
    isFree = True
    reachedDestination = False
    position = randint(0, len(list)-1)
    source = list[position].IP
    list.pop(position)
    position = randint(0, len(list)-1)
    destination = list[position].IP
    token = Token.Token(message, source, destination, reachedDestination, isFree)
    print("The source of the message is " + str(source) + " and the destination is " + str(destination))
    return token

def grabToken(computer, token):
    if computer.IP != token.source:
        if token.destination == computer.IP and token.reachedDestination == False:
            print()
            print("Reached destination")
            print()
            token.destination = token.source
            token.source = computer.IP
            token.reachedDestination = True
        elif token.destination == computer.IP and token.reachedDestination == True:
            print()
            print("Returned to source")
            print()
            token.destination = ""
            token.source = ""
        else:
            print("Computer " + str(computer.IP))
    return token


def main():
    threads = []
    computers = []
    for i in range(10):
        computer = Computer()
        computers.append(computer)
    global token
    token = initializeToken(computers)
    for comp in computers:
        thread = threading.Thread( target = grabToken, args = (comp, token))
        print(comp.IP)
        threads.append(thread)
    for thread in threads:
        thread.start()
    for thread in threads:
        thread.join()


if __name__ == '__main__':
    main()