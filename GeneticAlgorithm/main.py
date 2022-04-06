import random as rand

maxim = 0
popNumber = 0
nextGen = []
selected = []
population = []
fitness = []

def firstGen():
    global popNumber
    for i in range(int(popNumber)):
        x = rand.randint(-20,20)
        y = rand.randint(-5,5)
        population.append((x,y))
    print("Initial population: ", population)

def calclFitness():
    global maxim
    global popNumber
    global population
    fitnessSum = 0
    for person in population:
        x = person[0]
        y = person[1]
        calcl = abs(10*x*x*y - 5*x*x - 4*y*y - x*x*x*x - 2*y*y*y*y)/2+1
        if(calcl > maxim):
            maxim = calcl
        fitness.append(calcl)
        fitnessSum = fitnessSum + int(calcl)

    return fitnessSum

def selection(fitnessSum):
    global fitness
    global popNumber
    global selected
    global population
    # probability
    individualProbab = []
    for i in fitness:
        individualProbab.append(i / fitnessSum)

    # cumulative probability
    cumulativeProbab = []
    cumulativeProbab.append(individualProbab[0])
    limit = len(individualProbab)
    for i in range(1, limit):
        cumulativeProbab.append(cumulativeProbab[i-1] + individualProbab[i])

    # selection
    counter = 0
    k = 0
    while counter < int(popNumber) // 2:
        randNumber = rand.uniform(0, 1)
        for i in range(k, len(cumulativeProbab) - 1):
            if cumulativeProbab[i] <= randNumber and randNumber >= cumulativeProbab[i + 1]:
                selected.append(population[i + 1])
                break
        k = k + 1
        counter += 1
    fitness.clear()

def crossOver():
    global selected
    global nextGen
    global population

    crossover = 1
    # random numbers in [0, 1]
    randNumbers = []
    for i in range(len(selected)):
        randNum = rand.uniform(0, 1)
        randNumbers.append(randNum)
    #crossover
    for i in range(len(selected)-1):
        if randNumbers[i] < crossover:
            nextGen.append((selected[i][0], selected[i+1][1]))
            nextGen.append((selected[i+1][0], selected[i][1]))
        else:
            nextGen.append(selected[i])
    population = nextGen.copy()
    nextGen.clear()

def mutate(i):
    mutX = rand.uniform(1, 5)
    mutY = rand.uniform(1, 3)
    auxI = i[0]
    auxJ = i[1]
    if i[0] > 15:
        auxI -= mutX
    else:
        auxI += mutX
    if i[1] > 2.5:
        auxJ -= mutY
    else:
        auxJ += mutY
    return (auxI, auxJ)

def mutations():
    global population
    prob = 0.05
    auxPop = []
    for i in population:
        num = rand.uniform(0,1)
        if num < prob:
            auxPop.append(mutate(i))

def main():
    global maxim
    global popNumber
    popNumber = input()
    firstGen()
    for i in range(300):
        fitnessSum = calclFitness()
        selection(fitnessSum)
        print("Selected: ",selected)
        crossOver()
        print("Next generation: ", population)
        mutations()
    print(maxim)

if __name__ == main():
    main()