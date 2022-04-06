from random import randint

class Computer(object):
    def __init__(self):
        self.IP = self.generateIP()

    @classmethod
    def generateIP(self):
        ip = ""
        list = []
        for i in range(4):
            num = randint(0,255)
            list.append(num)
        for i in list:
            if ip != "":
                ip += "."
            ip += str(i)
        return ip

