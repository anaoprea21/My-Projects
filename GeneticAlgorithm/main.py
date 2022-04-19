import random as rand

maxim = 0
popNumber = 0
selected = []
population = []
fitness = []
firstAvr = 0
secondAvr = 1
xmax = 0
ymax = 0

def firstGen():
    global population
    global popNumber
    for i in range(int(popNumber)):
        x = rand.uniform(1, 3)
        y = rand.uniform(1, 3)
        population.append((x, y))
    print("Initial population: ", population)

def calclFitness():
    global maxim
    global popNumber
    global population
    global firstAvr
    global secondAvr
    global xmax
    global ymax
    genMaxim = 0
    fitnessSum = 0
    for person in population:
        x = person[0]
        y = person[1]
        calcl = ((1 / 2) * (x + y) - 1) / ((x * x) + (y * y))
        if(calcl > genMaxim):
            genMaxim = float(calcl)
            xmax = float(x)
            ymax = float(y)
        fitness.append(calcl)
        fitnessSum = fitnessSum + float(calcl)

    if(firstAvr == 0):
        firstAvr = genMaxim
    else:
        secondAvr = firstAvr
        firstAvr = genMaxim

    if (genMaxim > maxim):
        maxim = genMaxim

    return fitnessSum

def selection(fitnessSum):
    global fitness
    global popNumber
    global selected
    global population
    # probability per individual
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
            if cumulativeProbab[i] <= randNumber and randNumber <= cumulativeProbab[i + 1]:
                selected.append(population[i + 1])
                k = k + 1
                break
        counter += 1
    fitness.clear()

def crossOver():
    global selected
    nextGen = []
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
    print("Next generation: ", nextGen)
    nextGen.clear()
    selected.clear()

def mutate(pos):
    mutX = rand.uniform(0, 1)
    mutY = rand.uniform(0, 1)
    auxX = pos[0]
    auxY = pos[1]
    if pos[0] > 2:
        auxX -= mutX
    else:
        auxX += mutX
    if pos[1] > 2:
        auxY -= mutY
    else:
        auxY += mutY
    return (auxX, auxY)

def mutations():
    global population
    prob = 0.07
    auxPop = []
    for i in population:
        num = rand.uniform(0, 1)
        if num < prob:
            auxPop.append(mutate(i))
        else:
            auxPop.append(i)
    print("Population number: ", len(population))
    population = auxPop.copy()

def main():
    global maxim
    global population
    global selected
    global popNumber
    global firstAvr
    global secondAvr
    global xmax
    global ymax
    popNumber = 1000
    firstGen()
    generation = 0
    while abs(float(firstAvr) - float(secondAvr)) > 0.0000001:
        fitnessSum = float(calclFitness())
        selection(fitnessSum)
        print("Selected: ", selected)
        crossOver()
        mutations()
        generation += 1
    print("Maxim: ", maxim)
    print("X maxim: ", xmax)
    print("Y maxim: ", ymax)
    print ("Number of generations: ", generation)

if __name__ == main():
    main()