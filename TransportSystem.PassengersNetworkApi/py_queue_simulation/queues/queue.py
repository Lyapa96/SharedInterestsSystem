import typing
import numbers
from heapq import heappush
from numpy import infty


class QueueAssociatedEdge:
    """ Информация о ребре, которому сопоставлена очередь

    Parameters
        ----------
        from_vertex : int
            Номер вершины, из которой исходит ребро
        to_vertex : int
            Номер вершины, в которую ведет ребро
        index: int
            Индекс текущего ребра ``edge_index``
        type: int
            Тип текущего ребра (определяет тип СМО на данном ребре)
    """
    def __init__(self,
                 from_vertex: int = 0,
                 to_vertex: int = 0,
                 index: int = 0,
                 type: int = 1):

        self.from_vertex = from_vertex
        self.to_vertex = to_vertex
        self.index = index
        self.type = type

    @staticmethod
    def single_queue():
        return QueueAssociatedEdge()


class QueueNode:
    """ Базовый класс для СМО

    Может использоваться для моедлирования СМО с каналами, так и в
    сети из СМО :class:`.QueueNetwork`

    Parameters
    ----------
    num_servers : int или ``numpy.infty`` (optional, default: ``1``)
        Количество обслуживающих каналов.
    arrival_f : function (optional, default: ``lambda t: t + exponential(1)``)
        По текущему времени ``t`` функция вернет время следующего прибытия извне
        (не из QueueNetwork, если используется как часть сети)
    service_f : function (optional, default: ``lambda t: t + exponential(0.9)``)
        Функция, которая возвращает время, когда агент будет обслужен.
        При вызове аргумент ``t`` - время, когда агент начинает обслуживаться
    edge : :class:`~QueueAssociatedEdge` class (optional, default: ``QueueAssociatedEdge.single_queue()``)
        Ребро, которое ассоциировано с данной СМО
    AgentFactory : class (optional, default: the :class:`~Agent` class)
        Фабрика агентов для сети
    """

    def __init__(self,
                 num_servers: int = 1,
                 arrival_f: function = None,
                 service_f: function = None,
                 edge: QueueAssociatedEdge = QueueAssociatedEdge.single_queue(),
                 AgentFactory=Agent,
                 **kwargs):

        if not isinstance(num_servers, numbers.Integral) and num_servers is not infty:
            msg = "num_servers must be an integer or infinity."
            raise TypeError(msg)
        elif num_servers <= 0:
            msg = "num_servers must be a positive integer or infinity."
            raise ValueError(msg)

        self.num_servers = num_servers
        self.arrival_f = arrival_f
        self.service_f = service_f
        self.edge = edge


