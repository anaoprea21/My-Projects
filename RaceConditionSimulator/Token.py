class Token:
    def __init__(self, message, source, destination, reachedDestination, isFree):
        self.message = message
        self.source = source
        self.destination = destination
        self.reachedDestination = reachedDestination
        self.isFree = isFree

