import collections
import numbers
import copy
import array

import numpy as np
from numpy.random import uniform


class QueueNetwork:
    """Класс представляет собой сеть из СМО(:class:`.QueueNode`).
    """
