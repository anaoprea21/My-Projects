import threading
from threading import *
import time
import random as rand

condition = threading.Condition()
initMessage = "asd"
FIN = 0
number = 0
nextPos = 0
toBeRecivedMessage = ""



def send():
    global FIN,number,nextPos,toBeRecivedMessage,initMessage,condition
    while initMessage != "":
        print("Sender preparing to send an item: ")
        condition.acquire()
        condition.notifyAll()
        print("Sender awaiting")
        condition.wait()

        if number == 0:
            toBeRecivedMessage=""
        else:
            toBeRecivedMessage = initMessage[nextPos:number]
            initMessage=initMessage.replace(toBeRecivedMessage,"",1)
            print("SENDER INIT MESSAGE: ",initMessage)
        if initMessage == "":
            FIN = 1
            condition.notifyAll()
            condition.release()
            print("Finished sending")
            break





def recive():
    global FIN,number,nextPos,toBeRecivedMessage,initMessage,condition
    while FIN!=1:
        print("Recive prepering to recive an item")
        condition.acquire()
        number = rand.randint(0,len(initMessage)//2+1)
        print("Number is: ",number)
        condition.notifyAll()
        condition.wait()
        print("Message is: ",toBeRecivedMessage)
        if FIN == 1:
            break




def main():
    Thread(target=send).start()
    Thread(target=recive).start()

if _name_ == '_main_':
    main()