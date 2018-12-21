from numpy import argmin
from numpy.random import choice

from py_queue_simulation.network import QueueNetwork
from py_queue_simulation.queues import QueueAssociatedEdge, QueueNode


class Agent:
    """Базовый класс для Агента.

        ``Agents`` - это объекты, которые обслуживаются в СМО и перемещаются по сети из СМО.

        Parameters
        ----------
        agent_id : tuple (optional, default: ``(0, 0)``)
            Уникальный идентификатор агента внутри сети.
            При создании агента в :class:`.QueueServer` автоматически генерируется.
            После инициализации ``Agent`` не изменяется при движении по сети.
            Певрое значение в паре - идекс ребра :class:`QueueServer's<.QueueServer>`, на котором агент был создан
            Второе значение в паре - порядковый номер агента при создании в СМО

        Attributes
        ----------
        agent_id : tuple
            Уникальный идентификатор агента
        blocked : int
            Количестов раз, которое агент был заблокирован из-за конечного размера очереди в СМО
        """
    def __init__(self, agent_id: tuple = (0, 0)):
        self.agent_id = agent_id
        self.blocked = 0

        # Следующее время события для агента. Это может быть либо прибытие либо убытие
        self._time = 0

    def __repr__(self):
        return "Agent; agent_id:{0}. next time: {1}".format(self.agent_id, round(self._time, 5))

    def __lt__(self, b):
        return self._time < b._time

    def __gt__(self, b):
        return self._time > b._time

    def __eq__(self, b):
        return self._time == b._time

    def __le__(self, b):
        return self._time <= b._time

    def __ge__(self, b):
        return self._time >= b._time

    def add_loss(self, *args, **kwargs):
        """Каждый раз, когда агент после завершения не может попасть в следующую очередь
        делаем инкремент
        """
        self.blocked += 1

    def next_destination(self, network: QueueNetwork, edge: QueueAssociatedEdge) -> int:
        """На основании текущего положения в сети (текущего ребра)
        и конфигурации матрицы перехода в :class:`.QueueNetwork` получаем
        индекс следующего ребра для перехода

        ``Agent`` выбирает следующее ребро случайно из исходящих ребер данной вершины.
        Вероятность выбора ребра определяется на основании матрицы перехода из класса
        :class:`QueueNetwork` (transition matrix)

        Parameters
        ----------
        network : :class:`.QueueNetwork`
            Сеть из СМО :class:`.QueueNetwork` к которой относится агент.
        edge : :class:`.QueueAssociatedEdge`
            Ребро, на котором сейчас находится агент.

        Returns
        -------
        out : int
            Возвращает номер ребра, на которое перейдет агент в сети
        """

        # edge.to_vertex - куда дошли по текущему ребру
        out_edges_count = len(network.out_edges[edge.to_vertex])
        if out_edges_count <= 1:
            return network.out_edges[edge.to_vertex][0]

        # Если вариантов несколько
        # u = uniform(low=0.0, high=1.0)
        # Получаем массив вероятностей индексов ребер, куда пойдем, стоя на конце текущего ребра
        probabilities = network._route_probs[edge.to_vertex]

        # Получаем индекс ребра для перехода. Выбираем число от 0 до out_edges_count - 1
        # где вероятность выбора k равна probabilities[k]
        chosen_edge_index = choice(out_edges_count, p=probabilities)

        # Зная индекс выбранного ребра в массиве мы можем получить
        # глобальный порядковый номер ребра по массиву out_edges
        return network.out_edges[edge.to_vertex][chosen_edge_index]

    def queue_action(self, queue: QueueNode, *args, **kwargs):
        """Можем отследить события связанные с очередью, на которую попал агент
        Когда агент прибываем в СМО ``args[0] == 0``
        Когда агент начинает обслуживаться ``args[0] == 1``
        Когда агент убывает из очереди ``args[0] == 2``
        """
        pass


class GreedyAgent(Agent):
    """"Этот тип агента выбирает следующую СМО исходя из минимальности очереди ожидания
    """

    def __init__(self, agent_id: tuple = (0, 0)):
        Agent.__init__(self, agent_id)

    def __repr__(self):
        return "GreedyAgent; agent_id:{0}. next time: {1}".format(self.agent_id, round(self._time, 5))

    def desired_destination(self, network: QueueNetwork, edge: QueueAssociatedEdge) -> int:
        """На основании текущего положения в сети (текущего ребра)
        и конфигурации матрицы перехода в :class:`.QueueNetwork` получаем
        индекс следующего ребра для перехода

        ``GreedyAgents`` выбирает следующую СМО исходя из минимального
        количества :class:`Агентов<.Agent>` в очереди

        Parameters
        ----------
        network : :class:`.QueueNetwork`
            Сеть из СМО :class:`.QueueNetwork` к которой относится агент.
        edge : :class:`.QueueAssociatedEdge`
            Ребро, на котором сейчас находится агент.

        Returns
        -------
        out : int
            Возвращает номер ребра, на которое перейдет агент в сети
        """

        adjacent_edges = network.out_edges[edge.to_vertex]
        # По номеру исходящего ребра получаем очередь, которой он соответствует
        # Перебираем все очереди и формируем массив с длинной очереди на обслуживание
        # Берем индекс минимальной очереди
        min_queue_len_index = argmin([network.edge2queue[d].number_queued() for d in adjacent_edges])
        # По идексу получаем номер ребра, на которое перейдем
        return adjacent_edges[min_queue_len_index]
