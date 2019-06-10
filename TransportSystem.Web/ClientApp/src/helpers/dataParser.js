import {randomInteger} from './mathHelper';

export function transformData(data, channelsQuality) {
    //"0,0": [[0.03484852846663591, 0.03484852846663591, 0.04484852846663591, 0.0, 1.0, 1.0, 0.0], [0.04484852846663591, 0.04484852846663591, 0.13753919107576162, 0.0, 1.0, 1.0, 1.0], [0.13753919107576162, 0.0, 0.0, 0.0, 0.0, -1.0, 21.0]],
    //время прибытия агента
    //время начала обслуживания
    //время окончания обслуживания
    //размер очереди в момент прибытия агента
    //общее количество агентов в СМО
    //номер канала, на котором обслужился агент
    //номер ребра текщей СМО

    let result = [];
    for (let key in data) {
        let newObjects = data[key].map(x => {
            let channel = randomInteger(0, channelsQuality.length - 1);
            let quality = channelsQuality[channel];
            return {
                agentId: key,
                arriveAgentTime: Number(x[0]).toFixed(2),
                startTime: Number(x[1]).toFixed(2),
                endTime: Number(x[2]).toFixed(2),
                queueCount: x[3],
                agentsCount: x[4],
                channelNumber: channel,
                edgeNumber: x[6],
                quality: quality
            }
        });
        result = result.concat(newObjects);
    }

    return result;
}