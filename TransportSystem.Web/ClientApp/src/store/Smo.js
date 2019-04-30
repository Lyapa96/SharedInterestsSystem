import {transformData} from "../helpers/dataParser";

const setInitStateType = 'SET_INIT_STATE';
const getDataFromSockets = 'GET_DATA_FROM_SOCKETS';
const setSmoInteractiveMode = "SET_SMO_INTERACTIVE_MODE";
const runNextStepForSMO = "RUN_NEXT_STEP_FOR_SMO";

const initialState = {
    isInitState: true,
    smoResults: null,
    isInteractiveMode: false,
    smoSteps: []
};

export const actionCreators = {
    setInitState: () => ({type: setInitStateType}),
    getDataFromSockets: (data) => ({type: getDataFromSockets, payload: {...data, passengers: getData()}}),
    setSmoInteractiveMode: (data) => async (dispatch, getState) => {
        let smoPassengers = getState().smo.smoResults;
        const url = 'https://localhost:5003/api/passengers/smo';
        const response = await fetch(url, {
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            method: "POST",
            body: JSON.stringify({
                smoPassengers: smoPassengers,
                passengersOnCar: data.passengersOnCar,
                columns: data.columns,
                neighboursCount: 7
            })
        });
        const newPassengers = await response.json();
        dispatch({type: setSmoInteractiveMode, payload: newPassengers});
    },
    runNextStep: () => async (dispatch, getState) => {
        let smoState = getState().smo;
        let currentStep = smoState.smoSteps[smoState.smoSteps.length - 1];
        const url = 'https://localhost:5003/api/passengers/smoStep';
        const response = await fetch(url, {
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            method: "POST",
            body: JSON.stringify(currentStep)
        });
        const nextStep = await response.json();
        dispatch({type: runNextStepForSMO, payload: nextStep});
    }
};

function getData() {
    return {
        "0,0": [[0.03484852846663591, 0.03484852846663591, 0.04484852846663591, 0.0, 1.0, 1.0, 0.0], [0.04484852846663591, 0.04484852846663591, 0.13753919107576162, 0.0, 1.0, 1.0, 1.0], [0.13753919107576162, 0.0, 0.0, 0.0, 0.0, -1.0, 21.0]],
        "0,1": [[0.06430961281395668, 0.06430961281395668, 0.07430961281395668, 0.0, 1.0, 1.0, 0.0], [0.07430961281395668, 0.13753919107576162, 0.2032506271917212, 1.0, 2.0, 1.0, 1.0], [0.2032506271917212, 0.0, 0.0, 0.0, 0.0, -1.0, 21.0]],
        "0,2": [[0.10460774915368509, 0.10460774915368509, 0.11460774915368509, 0.0, 1.0, 1.0, 0.0], [0.11460774915368509, 0.11460774915368509, 0.2560438542052623, 0.0, 1.0, 1.0, 2.0], [0.2560438542052623, 0.0, 0.0, 0.0, 0.0, -1.0, 22.0]],
        "0,3": [[0.13103967665583782, 0.13103967665583782, 0.14103967665583783, 0.0, 1.0, 1.0, 0.0], [0.14103967665583783, 0.2032506271917212, 0.32921026536464504, 1.0, 2.0, 1.0, 1.0], [0.32921026536464504, 0.0, 0.0, 0.0, 0.0, -1.0, 21.0]],
        "0,4": [[0.13556201257211745, 0.14103967665583783, 0.15103967665583784, 1.0, 2.0, 1.0, 0.0], [0.15103967665583784, 0.2560438542052623, 0.2721497168947364, 1.0, 2.0, 1.0, 2.0], [0.2721497168947364, 0.0, 0.0, 0.0, 0.0, -1.0, 22.0]],
        "0,5": [[0.13869664734430828, 0.15103967665583784, 0.16103967665583785, 2.0, 3.0, 1.0, 0.0], [0.16103967665583785, 0.16103967665583785, 0.573576251439939, 0.0, 1.0, 1.0, 3.0], [0.573576251439939, 0.0, 0.0, 0.0, 0.0, -1.0, 23.0]],
        "0,6": [[0.1494631095781512, 0.16103967665583785, 0.17103967665583786, 2.0, 3.0, 1.0, 0.0], [0.17103967665583786, 0.573576251439939, 0.6358603099464332, 1.0, 2.0, 1.0, 3.0], [0.6358603099464332, 0.0, 0.0, 0.0, 0.0, -1.0, 23.0]],
        "0,7": [[0.15714228644931105, 0.17103967665583786, 0.18103967665583787, 2.0, 3.0, 1.0, 0.0], [0.18103967665583787, 0.18103967665583787, 0.23250722316818537, 0.0, 1.0, 1.0, 4.0], [0.23250722316818537, 0.0, 0.0, 0.0, 0.0, -1.0, 24.0]],
        "0,8": [[0.17266027276095192, 0.18103967665583787, 0.19103967665583788, 1.0, 2.0, 1.0, 0.0], [0.19103967665583788, 0.23250722316818537, 0.28627783610098734, 1.0, 2.0, 1.0, 4.0], [0.28627783610098734, 0.0, 0.0, 0.0, 0.0, -1.0, 24.0]],
        "0,9": [[0.20131988276250526, 0.20131988276250526, 0.21131988276250527, 0.0, 1.0, 1.0, 0.0], [0.21131988276250527, 0.32921026536464504, 0.379175476916113, 1.0, 2.0, 1.0, 1.0], [0.379175476916113, 0.0, 0.0, 0.0, 0.0, -1.0, 21.0]],
        "0,10": [[0.2179476556973862, 0.2179476556973862, 0.2279476556973862, 0.0, 1.0, 1.0, 0.0], [0.2279476556973862, 0.2279476556973862, 0.23663615041763675, 0.0, 1.0, 1.0, 5.0], [0.23663615041763675, 0.0, 0.0, 0.0, 0.0, -1.0, 25.0]],
        "0,11": [[0.23866380665783227, 0.23866380665783227, 0.24866380665783228, 0.0, 1.0, 1.0, 0.0], [0.24866380665783228, 0.28627783610098734, 0.34073647447543604, 1.0, 2.0, 1.0, 4.0], [0.34073647447543604, 0.0, 0.0, 0.0, 0.0, -1.0, 24.0]],
        "0,12": [[0.2458543683454836, 0.24866380665783228, 0.2586638066578323, 1.0, 2.0, 1.0, 0.0], [0.2586638066578323, 0.2721497168947364, 0.3308521518400408, 1.0, 2.0, 1.0, 2.0], [0.3308521518400408, 0.0, 0.0, 0.0, 0.0, -1.0, 22.0]],
        "0,13": [[0.2589853558262673, 0.2589853558262673, 0.2689853558262673, 0.0, 1.0, 1.0, 0.0], [0.2689853558262673, 0.2689853558262673, 0.2742125810492459, 0.0, 1.0, 1.0, 5.0], [0.2742125810492459, 0.0, 0.0, 0.0, 0.0, -1.0, 25.0]],
        "0,14": [[0.2663440102288036, 0.2689853558262673, 0.2789853558262673, 1.0, 2.0, 1.0, 0.0], [0.2789853558262673, 0.3308521518400408, 0.366094614636315, 1.0, 2.0, 1.0, 2.0], [0.366094614636315, 0.0, 0.0, 0.0, 0.0, -1.0, 22.0]],
        "0,15": [[0.270410650380823, 0.2789853558262673, 0.28898535582626733, 1.0, 2.0, 1.0, 0.0], [0.28898535582626733, 0.34073647447543604, 0.5422562057976335, 1.0, 2.0, 1.0, 4.0], [0.5422562057976335, 0.0, 0.0, 0.0, 0.0, -1.0, 24.0]],
        "0,16": [[0.29475305296368604, 0.29475305296368604, 0.30475305296368604, 0.0, 1.0, 1.0, 0.0], [0.30475305296368604, 0.30475305296368604, 0.30670949870207154, 0.0, 1.0, 1.0, 5.0], [0.30670949870207154, 0.0, 0.0, 0.0, 0.0, -1.0, 25.0]],
        "0,17": [[0.2953956579626452, 0.30475305296368604, 0.31475305296368605, 1.0, 2.0, 1.0, 0.0], [0.31475305296368605, 0.31475305296368605, 0.3236302139948102, 0.0, 1.0, 1.0, 5.0], [0.3236302139948102, 0.0, 0.0, 0.0, 0.0, -1.0, 25.0]],
        "0,18": [[0.29739186689739633, 0.31475305296368605, 0.32475305296368606, 2.0, 3.0, 1.0, 0.0], [0.32475305296368606, 0.32475305296368606, 0.33579800128230247, 0.0, 1.0, 1.0, 5.0], [0.33579800128230247, 0.0, 0.0, 0.0, 0.0, -1.0, 25.0]],
        "0,19": [[0.29828938867628224, 0.32475305296368606, 0.33475305296368607, 3.0, 4.0, 1.0, 0.0], [0.33475305296368607, 0.379175476916113, 0.40729290245932687, 1.0, 2.0, 1.0, 1.0], [0.40729290245932687, 0.0, 0.0, 0.0, 0.0, -1.0, 21.0]],
        "0,20": [[0.30460431382344344, 0.33475305296368607, 0.3447530529636861, 4.0, 5.0, 1.0, 0.0], [0.3447530529636861, 0.366094614636315, 0.3782539197698767, 1.0, 2.0, 1.0, 2.0], [0.3782539197698767, 0.0, 0.0, 0.0, 0.0, -1.0, 22.0]],
        "0,21": [[0.35071824586765754, 0.35071824586765754, 0.36071824586765755, 0.0, 1.0, 1.0, 0.0], [0.36071824586765755, 0.5422562057976335, 0.5761790579016659, 1.0, 2.0, 1.0, 4.0], [0.5761790579016659, 0.0, 0.0, 0.0, 0.0, -1.0, 24.0]],
        "0,22": [[0.35139824081641124, 0.36071824586765755, 0.37071824586765756, 1.0, 2.0, 1.0, 0.0], [0.37071824586765756, 0.3782539197698767, 0.41705852618603395, 1.0, 2.0, 1.0, 2.0], [0.41705852618603395, 0.0, 0.0, 0.0, 0.0, -1.0, 22.0]],
        "0,23": [[0.35785555784568246, 0.37071824586765756, 0.38071824586765757, 2.0, 3.0, 1.0, 0.0], [0.38071824586765757, 0.40729290245932687, 0.4246501591106884, 1.0, 2.0, 1.0, 1.0], [0.4246501591106884, 0.0, 0.0, 0.0, 0.0, -1.0, 21.0]],
        "0,24": [[0.3690418732083997, 0.38071824586765757, 0.3907182458676576, 2.0, 3.0, 1.0, 0.0], [0.3907182458676576, 0.41705852618603395, 0.587790104515087, 1.0, 2.0, 1.0, 2.0], [0.587790104515087, 0.0, 0.0, 0.0, 0.0, -1.0, 22.0]],
        "0,25": [[0.38015728815314115, 0.3907182458676576, 0.4007182458676576, 2.0, 3.0, 1.0, 0.0], [0.4007182458676576, 0.4007182458676576, 0.6073060368410304, 0.0, 1.0, 1.0, 5.0], [0.6073060368410304, 0.0, 0.0, 0.0, 0.0, -1.0, 25.0]],
        "0,26": [[0.38815561826803513, 0.4007182458676576, 0.4107182458676576, 2.0, 3.0, 1.0, 0.0], [0.4107182458676576, 0.4246501591106884, 0.6327121624466312, 1.0, 2.0, 1.0, 1.0], [0.6327121624466312, 0.0, 0.0, 0.0, 0.0, -1.0, 21.0]],
        "0,27": [[0.3893520394766912, 0.4107182458676576, 0.4207182458676576, 3.0, 4.0, 1.0, 0.0], [0.4207182458676576, 0.587790104515087, 0.6075628064513761, 1.0, 2.0, 1.0, 2.0], [0.6075628064513761, 0.0, 0.0, 0.0, 0.0, -1.0, 22.0]],
        "0,28": [[0.40306758153804656, 0.4207182458676576, 0.4307182458676576, 2.0, 3.0, 1.0, 0.0], [0.4307182458676576, 0.6327121624466312, 0.7160517405826281, 1.0, 2.0, 1.0, 1.0], [0.7160517405826281, 0.0, 0.0, 0.0, 0.0, -1.0, 21.0]],
        "0,29": [[0.41676943369623165, 0.4307182458676576, 0.4407182458676576, 2.0, 3.0, 1.0, 0.0], [0.4407182458676576, 0.6073060368410304, 0.6084736897076657, 1.0, 2.0, 1.0, 5.0], [0.6084736897076657, 0.0, 0.0, 0.0, 0.0, -1.0, 25.0]],
        "0,30": [[0.4370282903159882, 0.4407182458676576, 0.45071824586765763, 1.0, 2.0, 1.0, 0.0], [0.45071824586765763, 0.45071824586765763, 0.45390481253737874, 0.0, 1.0, 1.0, 6.0], [0.45390481253737874, 0.0, 0.0, 0.0, 0.0, -1.0, 26.0]],
        "0,31": [[0.43830025554449004, 0.45071824586765763, 0.46071824586765764, 2.0, 3.0, 1.0, 0.0], [0.46071824586765764, 0.46071824586765764, 0.47680068814849974, 0.0, 1.0, 1.0, 6.0], [0.47680068814849974, 0.0, 0.0, 0.0, 0.0, -1.0, 26.0]],
        "0,32": [[0.44906032359189446, 0.46071824586765764, 0.47071824586765765, 2.0, 3.0, 1.0, 0.0], [0.47071824586765765, 0.47680068814849974, 0.507040295501114, 1.0, 2.0, 1.0, 6.0], [0.507040295501114, 0.0, 0.0, 0.0, 0.0, -1.0, 26.0]],
        "0,33": [[0.45377658853098807, 0.47071824586765765, 0.48071824586765766, 2.0, 3.0, 1.0, 0.0], [0.48071824586765766, 0.507040295501114, 0.5237576317132555, 1.0, 2.0, 1.0, 6.0], [0.5237576317132555, 0.0, 0.0, 0.0, 0.0, -1.0, 26.0]],
        "0,34": [[0.45781604453165625, 0.48071824586765766, 0.49071824586765767, 3.0, 4.0, 1.0, 0.0], [0.49071824586765767, 0.49071824586765767, 0.4927122227287492, 0.0, 1.0, 1.0, 7.0], [0.4927122227287492, 0.0, 0.0, 0.0, 0.0, -1.0, 27.0]],
        "0,35": [[0.49365033816415654, 0.49365033816415654, 0.5036503381641565, 0.0, 1.0, 1.0, 0.0], [0.5036503381641565, 0.5036503381641565, 0.5134578897472345, 0.0, 1.0, 1.0, 7.0], [0.5134578897472345, 0.0, 0.0, 0.0, 0.0, -1.0, 27.0]],
        "0,36": [[0.5073121709353267, 0.5073121709353267, 0.5173121709353267, 0.0, 1.0, 1.0, 0.0], [0.5173121709353267, 0.5237576317132555, 0.8721813495320647, 1.0, 2.0, 1.0, 6.0], [0.8721813495320647, 0.0, 0.0, 0.0, 0.0, -1.0, 26.0]],
        "0,37": [[0.5129742600743626, 0.5173121709353267, 0.5273121709353267, 1.0, 2.0, 1.0, 0.0], [0.5273121709353267, 0.8721813495320647, 0.9713290043443636, 1.0, 2.0, 1.0, 6.0], [0.9713290043443636, 0.0, 0.0, 0.0, 0.0, -1.0, 26.0]],
        "0,38": [[0.513625042189971, 0.5273121709353267, 0.5373121709353267, 2.0, 3.0, 1.0, 0.0], [0.5373121709353267, 0.5373121709353267, 0.5571583755356072, 0.0, 1.0, 1.0, 7.0], [0.5571583755356072, 0.0, 0.0, 0.0, 0.0, -1.0, 27.0]],
        "0,39": [[0.5153981714020806, 0.5373121709353267, 0.5473121709353267, 3.0, 4.0, 1.0, 0.0], [0.5473121709353267, 0.5761790579016659, 0.5997191652744699, 1.0, 2.0, 1.0, 4.0], [0.5997191652744699, 0.0, 0.0, 0.0, 0.0, -1.0, 24.0]],
        "0,40": [[0.5181703321706532, 0.5473121709353267, 0.5573121709353267, 3.0, 4.0, 1.0, 0.0], [0.5573121709353267, 0.5573121709353267, 0.5657763043857454, 0.0, 1.0, 1.0, 7.0], [0.5657763043857454, 0.0, 0.0, 0.0, 0.0, -1.0, 27.0]],
        "0,41": [[0.5231411898104402, 0.5573121709353267, 0.5673121709353267, 4.0, 5.0, 1.0, 0.0], [0.5673121709353267, 0.5673121709353267, 0.7318791672789257, 0.0, 1.0, 1.0, 7.0], [0.7318791672789257, 0.0, 0.0, 0.0, 0.0, -1.0, 27.0]],
        "0,42": [[0.5237419014558777, 0.5673121709353267, 0.5773121709353267, 5.0, 6.0, 1.0, 0.0], [0.5773121709353267, 0.6358603099464332, 0.8538361544256113, 1.0, 2.0, 1.0, 3.0], [0.8538361544256113, 0.0, 0.0, 0.0, 0.0, -1.0, 23.0]],
        "0,43": [[0.5284962844062139, 0.5773121709353267, 0.5873121709353267, 5.0, 6.0, 1.0, 0.0], [0.5873121709353267, 0.5997191652744699, 0.618819215154499, 1.0, 2.0, 1.0, 4.0], [0.618819215154499, 0.0, 0.0, 0.0, 0.0, -1.0, 24.0]],
        "0,44": [[0.5341824613473332, 0.5873121709353267, 0.5973121709353267, 6.0, 7.0, 1.0, 0.0], [0.5973121709353267, 0.6075628064513761, 0.6575253465321419, 1.0, 2.0, 1.0, 2.0], [0.6575253465321419, 0.0, 0.0, 0.0, 0.0, -1.0, 22.0]],
        "0,45": [[0.5427281862896024, 0.5973121709353267, 0.6073121709353267, 6.0, 7.0, 1.0, 0.0], [0.6073121709353267, 0.618819215154499, 0.6828311026256143, 1.0, 2.0, 1.0, 4.0], [0.6828311026256143, 0.0, 0.0, 0.0, 0.0, -1.0, 24.0]],
        "0,46": [[0.545008705465928, 0.6073121709353267, 0.6173121709353268, 7.0, 8.0, 1.0, 0.0], [0.6173121709353268, 0.6575253465321419, 0.7930857728744956, 1.0, 2.0, 1.0, 2.0], [0.7930857728744956, 0.0, 0.0, 0.0, 0.0, -1.0, 22.0]],
        "0,47": [[0.5487730774124993, 0.6173121709353268, 0.6273121709353268, 7.0, 8.0, 1.0, 0.0], [0.6273121709353268, 0.6828311026256143, 0.7050192658723016, 1.0, 2.0, 1.0, 4.0], [0.7050192658723016, 0.0, 0.0, 0.0, 0.0, -1.0, 24.0]],
        "0,48": [[0.5705672382396184, 0.6273121709353268, 0.6373121709353268, 6.0, 7.0, 1.0, 0.0], [0.6373121709353268, 0.7160517405826281, 0.7350466324773222, 1.0, 2.0, 1.0, 1.0], [0.7350466324773222, 0.0, 0.0, 0.0, 0.0, -1.0, 21.0]],
        "0,49": [[0.5709448466727038, 0.6373121709353268, 0.6473121709353268, 7.0, 8.0, 1.0, 0.0], [0.6473121709353268, 0.8538361544256113, 0.856425933009482, 1.0, 2.0, 1.0, 3.0], [0.856425933009482, 0.0, 0.0, 0.0, 0.0, -1.0, 23.0]],
        "0,50": [[0.5726089339703798, 0.6473121709353268, 0.6573121709353268, 8.0, 9.0, 1.0, 0.0], [0.6573121709353268, 0.6573121709353268, 0.7963621636102431, 0.0, 1.0, 1.0, 5.0], [0.7963621636102431, 0.0, 0.0, 0.0, 0.0, -1.0, 25.0]],
        "0,51": [[0.5947490473699882, 0.6573121709353268, 0.6673121709353268, 7.0, 8.0, 1.0, 0.0], [0.6673121709353268, 0.7930857728744956, 0.798108491400051, 1.0, 2.0, 1.0, 2.0], [0.798108491400051, 0.0, 0.0, 0.0, 0.0, -1.0, 22.0]],
        "0,52": [[0.5979834077605486, 0.6673121709353268, 0.6773121709353268, 7.0, 8.0, 1.0, 0.0], [0.6773121709353268, 0.7963621636102431, 0.8785291493908708, 1.0, 2.0, 1.0, 5.0], [0.8785291493908708, 0.0, 0.0, 0.0, 0.0, -1.0, 25.0]],
        "0,53": [[0.6041687355911116, 0.6773121709353268, 0.6873121709353268, 8.0, 9.0, 1.0, 0.0], [0.6873121709353268, 0.7050192658723016, 0.8090551126153007, 1.0, 2.0, 1.0, 4.0], [0.8090551126153007, 0.0, 0.0, 0.0, 0.0, -1.0, 24.0]],
        "0,54": [[0.605284491159784, 0.6873121709353268, 0.6973121709353268, 9.0, 10.0, 1.0, 0.0], [0.6973121709353268, 0.7318791672789257, 0.7379547630637959, 1.0, 2.0, 1.0, 7.0], [0.7379547630637959, 0.0, 0.0, 0.0, 0.0, -1.0, 27.0]],
        "0,55": [[0.6075447429893217, 0.6973121709353268, 0.7073121709353268, 9.0, 10.0, 1.0, 0.0], [0.7073121709353268, 0.8090551126153007, 0.8272490827610213, 1.0, 2.0, 1.0, 4.0], [0.8272490827610213, 0.0, 0.0, 0.0, 0.0, -1.0, 24.0]],
        "0,56": [[0.6077003045994623, 0.7073121709353268, 0.7173121709353268, 10.0, 11.0, 1.0, 0.0], [0.7173121709353268, 0.7350466324773222, 0.7854719772463498, 1.0, 2.0, 1.0, 1.0], [0.7854719772463498, 0.0, 0.0, 0.0, 0.0, -1.0, 21.0]],
        "0,57": [[0.6086726379228233, 0.7173121709353268, 0.7273121709353269, 11.0, 12.0, 1.0, 0.0], [0.7273121709353269, 0.7273121709353269, 0.9439431756174046, 0.0, 1.0, 1.0, 8.0], [0.9439431756174046, 0.0, 0.0, 0.0, 0.0, -1.0, 28.0]],
        "0,58": [[0.6088040613867147, 0.7273121709353269, 0.7373121709353269, 12.0, 13.0, 1.0, 0.0], [0.7373121709353269, 0.7854719772463498, 0.7943711474737828, 1.0, 2.0, 1.0, 1.0], [0.7943711474737828, 0.0, 0.0, 0.0, 0.0, -1.0, 21.0]],
        "0,59": [[0.6159485672127886, 0.7373121709353269, 0.7473121709353269, 13.0, 14.0, 1.0, 0.0], [0.7473121709353269, 0.7473121709353269, 0.7480198915308577, 0.0, 1.0, 1.0, 7.0], [0.7480198915308577, 0.0, 0.0, 0.0, 0.0, -1.0, 27.0]],
        "0,60": [[0.6166343196668419, 0.7473121709353269, 0.7573121709353269, 14.0, 15.0, 1.0, 0.0], [0.7573121709353269, 0.7573121709353269, 0.0, 0.0, 1.0, 1.0, 7.0]],
        "0,61": [[0.6238882899814421, 0.7573121709353269, 0.7673121709353269, 14.0, 15.0, 1.0, 0.0], [0.7673121709353269, 0.0, 0.0, 1.0, 2.0, -1.0, 7.0]],
        "0,62": [[0.6244778317784077, 0.7673121709353269, 0.7773121709353269, 15.0, 16.0, 1.0, 0.0], [0.7773121709353269, 0.9439431756174046, 0.0, 1.0, 2.0, 1.0, 8.0]],
        "0,63": [[0.6289719491084043, 0.7773121709353269, 0.7873121709353269, 15.0, 16.0, 1.0, 0.0], [0.7873121709353269, 0.7943711474737828, 0.7968328318035789, 1.0, 2.0, 1.0, 1.0], [0.7968328318035789, 0.0, 0.0, 0.0, 0.0, -1.0, 21.0]],
        "0,64": [[0.6307364077800783, 0.7873121709353269, 0.7973121709353269, 16.0, 17.0, 1.0, 0.0], [0.7973121709353269, 0.7973121709353269, 0.808295631083959, 0.0, 1.0, 1.0, 1.0], [0.808295631083959, 0.0, 0.0, 0.0, 0.0, -1.0, 21.0]],
        "0,65": [[0.636133810682371, 0.7973121709353269, 0.8073121709353269, 17.0, 18.0, 1.0, 0.0], [0.8073121709353269, 0.808295631083959, 0.8320269705063411, 1.0, 2.0, 1.0, 1.0], [0.8320269705063411, 0.0, 0.0, 0.0, 0.0, -1.0, 21.0]],
        "0,66": [[0.6371935338469372, 0.8073121709353269, 0.8173121709353269, 18.0, 19.0, 1.0, 0.0], [0.8173121709353269, 0.8320269705063411, 0.8621110106643172, 1.0, 2.0, 1.0, 1.0], [0.8621110106643172, 0.0, 0.0, 0.0, 0.0, -1.0, 21.0]],
        "0,67": [[0.6422696296595011, 0.8173121709353269, 0.8273121709353269, 18.0, 19.0, 1.0, 0.0], [0.8273121709353269, 0.8273121709353269, 0.9603062770933317, 0.0, 1.0, 1.0, 2.0], [0.9603062770933317, 0.0, 0.0, 0.0, 0.0, -1.0, 22.0]],
        "0,68": [[0.6517800848705073, 0.8273121709353269, 0.837312170935327, 18.0, 19.0, 1.0, 0.0], [0.837312170935327, 0.8621110106643172, 0.9725644050393852, 1.0, 2.0, 1.0, 1.0], [0.9725644050393852, 0.0, 0.0, 0.0, 0.0, -1.0, 21.0]],
        "0,69": [[0.6541285118623501, 0.837312170935327, 0.847312170935327, 19.0, 20.0, 1.0, 0.0], [0.847312170935327, 0.9603062770933317, 0.0, 1.0, 2.0, 1.0, 2.0]],
        "0,70": [[0.658882440460514, 0.847312170935327, 0.857312170935327, 19.0, 20.0, 1.0, 0.0], [0.857312170935327, 0.857312170935327, 0.9503035716813618, 0.0, 1.0, 1.0, 3.0], [0.9503035716813618, 0.0, 0.0, 0.0, 0.0, -1.0, 23.0]],
        "0,71": [[0.6601312122798861, 0.857312170935327, 0.867312170935327, 20.0, 21.0, 1.0, 0.0], [0.867312170935327, 0.9725644050393852, 0.0, 1.0, 2.0, 1.0, 1.0]],
        "0,72": [[0.6756208531639639, 0.867312170935327, 0.877312170935327, 20.0, 21.0, 1.0, 0.0], [0.877312170935327, 0.9503035716813618, 0.0, 1.0, 2.0, 1.0, 3.0]],
        "0,73": [[0.6772859439762918, 0.877312170935327, 0.887312170935327, 21.0, 22.0, 1.0, 0.0], [0.887312170935327, 0.887312170935327, 0.9207451117194722, 0.0, 1.0, 1.0, 4.0], [0.9207451117194722, 0.0, 0.0, 0.0, 0.0, -1.0, 24.0]],
        "0,74": [[0.6822565766171446, 0.887312170935327, 0.897312170935327, 21.0, 22.0, 1.0, 0.0], [0.897312170935327, 0.9207451117194722, 0.0, 1.0, 2.0, 1.0, 4.0]],
        "0,75": [[0.6889220376617535, 0.897312170935327, 0.907312170935327, 21.0, 22.0, 1.0, 0.0], [0.907312170935327, 0.907312170935327, 0.9428866273744194, 0.0, 1.0, 1.0, 5.0], [0.9428866273744194, 0.0, 0.0, 0.0, 0.0, -1.0, 25.0]],
        "0,76": [[0.6889370239330221, 0.907312170935327, 0.917312170935327, 22.0, 23.0, 1.0, 0.0], [0.917312170935327, 0.9428866273744194, 0.0, 1.0, 2.0, 1.0, 5.0]],
        "0,77": [[0.6946094297848675, 0.917312170935327, 0.927312170935327, 23.0, 24.0, 1.0, 0.0], [0.927312170935327, 0.0, 0.0, 1.0, 2.0, -1.0, 4.0]],
        "0,78": [[0.6993713701963669, 0.927312170935327, 0.937312170935327, 23.0, 24.0, 1.0, 0.0], [0.937312170935327, 0.9713290043443636, 0.0, 1.0, 2.0, 1.0, 6.0]],
        "0,79": [[0.7000223258953451, 0.937312170935327, 0.947312170935327, 24.0, 25.0, 1.0, 0.0], [0.947312170935327, 0.0, 0.0, 1.0, 2.0, -1.0, 5.0]],
        "0,80": [[0.7018047942664712, 0.947312170935327, 0.9573121709353271, 25.0, 26.0, 1.0, 0.0], [0.9573121709353271, 0.0, 0.0, 1.0, 2.0, -1.0, 3.0]],
        "0,81": [[0.7110034797943998, 0.9573121709353271, 0.9673121709353271, 25.0, 26.0, 1.0, 0.0], [0.9673121709353271, 0.0, 0.0, 1.0, 2.0, -1.0, 2.0]],
        "0,82": [[0.7246162295144432, 0.9673121709353271, 0.9773121709353271, 25.0, 26.0, 1.0, 0.0], [0.9773121709353271, 0.0, 0.0, 1.0, 2.0, -1.0, 1.0]],
        "0,83": [[0.7297407416898006, 0.9773121709353271, 0.9873121709353271, 25.0, 26.0, 1.0, 0.0], [0.9873121709353271, 0.0, 0.0, 1.0, 2.0, -1.0, 6.0]],
        "0,84": [[0.7302359586593372, 0.9873121709353271, 0.9973121709353271, 26.0, 27.0, 1.0, 0.0], [0.9973121709353271, 0.0, 0.0, 1.0, 2.0, -1.0, 8.0]],
        "0,85": [[0.7326423071543633, 0.9973121709353271, 0.0, 27.0, 28.0, 1.0, 0.0]],
        "0,86": [[0.7329507091946412, 0.0, 0.0, 28.0, 29.0, -1.0, 0.0]],
        "0,87": [[0.7344293567092925, 0.0, 0.0, 29.0, 30.0, -1.0, 0.0]],
        "0,88": [[0.7389828197120483, 0.0, 0.0, 29.0, 30.0, -1.0, 0.0]],
        "0,89": [[0.7390442333712718, 0.0, 0.0, 30.0, 31.0, -1.0, 0.0]],
        "0,90": [[0.7392113872771608, 0.0, 0.0, 31.0, 32.0, -1.0, 0.0]],
        "0,91": [[0.7404548862511325, 0.0, 0.0, 32.0, 33.0, -1.0, 0.0]],
        "0,92": [[0.7444419827287796, 0.0, 0.0, 33.0, 34.0, -1.0, 0.0]],
        "0,93": [[0.7491758838716902, 0.0, 0.0, 33.0, 34.0, -1.0, 0.0]],
        "0,94": [[0.7503178171677629, 0.0, 0.0, 34.0, 35.0, -1.0, 0.0]],
        "0,95": [[0.75271223920967, 0.0, 0.0, 35.0, 36.0, -1.0, 0.0]],
        "0,96": [[0.7532545147122444, 0.0, 0.0, 36.0, 37.0, -1.0, 0.0]],
        "0,97": [[0.7536761960674367, 0.0, 0.0, 37.0, 38.0, -1.0, 0.0]],
        "0,98": [[0.7573278318236625, 0.0, 0.0, 37.0, 38.0, -1.0, 0.0]],
        "0,99": [[0.7591359332176115, 0.0, 0.0, 38.0, 39.0, -1.0, 0.0]],
        "0,100": [[0.7606490315595533, 0.0, 0.0, 39.0, 40.0, -1.0, 0.0]],
        "0,101": [[0.7635230437693709, 0.0, 0.0, 40.0, 41.0, -1.0, 0.0]],
        "0,102": [[0.7696083528302207, 0.0, 0.0, 40.0, 41.0, -1.0, 0.0]],
        "0,103": [[0.7769620533168107, 0.0, 0.0, 41.0, 42.0, -1.0, 0.0]],
        "0,104": [[0.7775960408209334, 0.0, 0.0, 41.0, 42.0, -1.0, 0.0]],
        "0,105": [[0.7783036033323822, 0.0, 0.0, 42.0, 43.0, -1.0, 0.0]],
        "0,106": [[0.779149465931403, 0.0, 0.0, 43.0, 44.0, -1.0, 0.0]],
        "0,107": [[0.7797124685136647, 0.0, 0.0, 44.0, 45.0, -1.0, 0.0]],
        "0,108": [[0.7804481728169168, 0.0, 0.0, 45.0, 46.0, -1.0, 0.0]],
        "0,109": [[0.7830445899083796, 0.0, 0.0, 46.0, 47.0, -1.0, 0.0]],
        "0,110": [[0.7868782840557514, 0.0, 0.0, 47.0, 48.0, -1.0, 0.0]],
        "0,111": [[0.787474703230542, 0.0, 0.0, 47.0, 48.0, -1.0, 0.0]],
        "0,112": [[0.7880925628992642, 0.0, 0.0, 48.0, 49.0, -1.0, 0.0]],
        "0,113": [[0.7890240712120452, 0.0, 0.0, 49.0, 50.0, -1.0, 0.0]],
        "0,114": [[0.792582875543977, 0.0, 0.0, 50.0, 51.0, -1.0, 0.0]],
        "0,115": [[0.7947838577682076, 0.0, 0.0, 51.0, 52.0, -1.0, 0.0]],
        "0,116": [[0.7962290416067582, 0.0, 0.0, 52.0, 53.0, -1.0, 0.0]],
        "0,117": [[0.7969698989634075, 0.0, 0.0, 53.0, 54.0, -1.0, 0.0]],
        "0,118": [[0.7971321849152566, 0.0, 0.0, 54.0, 55.0, -1.0, 0.0]],
        "0,119": [[0.8050792022004389, 0.0, 0.0, 54.0, 55.0, -1.0, 0.0]],
        "0,120": [[0.8127459013432331, 0.0, 0.0, 54.0, 55.0, -1.0, 0.0]],
        "0,121": [[0.8135694342472666, 0.0, 0.0, 55.0, 56.0, -1.0, 0.0]],
        "0,122": [[0.8137977186102178, 0.0, 0.0, 56.0, 57.0, -1.0, 0.0]],
        "0,123": [[0.8176281962109393, 0.0, 0.0, 56.0, 57.0, -1.0, 0.0]],
        "0,124": [[0.8211633851627208, 0.0, 0.0, 57.0, 58.0, -1.0, 0.0]],
        "0,125": [[0.8232720315016214, 0.0, 0.0, 58.0, 59.0, -1.0, 0.0]],
        "0,126": [[0.8291163627163681, 0.0, 0.0, 58.0, 59.0, -1.0, 0.0]],
        "0,127": [[0.830083707910383, 0.0, 0.0, 59.0, 60.0, -1.0, 0.0]],
        "0,128": [[0.8367760581933967, 0.0, 0.0, 60.0, 61.0, -1.0, 0.0]],
        "0,129": [[0.8391750099051782, 0.0, 0.0, 60.0, 61.0, -1.0, 0.0]],
        "0,130": [[0.8418126296787397, 0.0, 0.0, 61.0, 62.0, -1.0, 0.0]],
        "0,131": [[0.8556687722016918, 0.0, 0.0, 61.0, 62.0, -1.0, 0.0]],
        "0,132": [[0.8585234124970678, 0.0, 0.0, 61.0, 62.0, -1.0, 0.0]],
        "0,133": [[0.8602959838763197, 0.0, 0.0, 62.0, 63.0, -1.0, 0.0]],
        "0,134": [[0.8620049652527582, 0.0, 0.0, 63.0, 64.0, -1.0, 0.0]],
        "0,135": [[0.8627534021033879, 0.0, 0.0, 64.0, 65.0, -1.0, 0.0]],
        "0,136": [[0.863518263959295, 0.0, 0.0, 65.0, 66.0, -1.0, 0.0]],
        "0,137": [[0.870630578809324, 0.0, 0.0, 65.0, 66.0, -1.0, 0.0]],
        "0,138": [[0.8725286218090336, 0.0, 0.0, 66.0, 67.0, -1.0, 0.0]],
        "0,139": [[0.8729782218676749, 0.0, 0.0, 67.0, 68.0, -1.0, 0.0]],
        "0,140": [[0.8778241586088662, 0.0, 0.0, 67.0, 68.0, -1.0, 0.0]],
        "0,141": [[0.8827718931953044, 0.0, 0.0, 68.0, 69.0, -1.0, 0.0]],
        "0,142": [[0.8871637097365662, 0.0, 0.0, 69.0, 70.0, -1.0, 0.0]],
        "0,143": [[0.8878049099645767, 0.0, 0.0, 69.0, 70.0, -1.0, 0.0]],
        "0,144": [[0.8890197721105725, 0.0, 0.0, 70.0, 71.0, -1.0, 0.0]],
        "0,145": [[0.8942293136918456, 0.0, 0.0, 71.0, 72.0, -1.0, 0.0]],
        "0,146": [[0.894650145068649, 0.0, 0.0, 72.0, 73.0, -1.0, 0.0]],
        "0,147": [[0.8962966990732247, 0.0, 0.0, 73.0, 74.0, -1.0, 0.0]],
        "0,148": [[0.897009623647456, 0.0, 0.0, 74.0, 75.0, -1.0, 0.0]],
        "0,149": [[0.8992891824418869, 0.0, 0.0, 74.0, 75.0, -1.0, 0.0]],
        "0,150": [[0.8996527499658996, 0.0, 0.0, 75.0, 76.0, -1.0, 0.0]],
        "0,151": [[0.9018352800394515, 0.0, 0.0, 76.0, 77.0, -1.0, 0.0]],
        "0,152": [[0.9082465398148619, 0.0, 0.0, 76.0, 77.0, -1.0, 0.0]],
        "0,153": [[0.9084325413813132, 0.0, 0.0, 77.0, 78.0, -1.0, 0.0]],
        "0,154": [[0.9097151166063222, 0.0, 0.0, 78.0, 79.0, -1.0, 0.0]],
        "0,155": [[0.9112079786497955, 0.0, 0.0, 79.0, 80.0, -1.0, 0.0]],
        "0,156": [[0.9124831954851766, 0.0, 0.0, 80.0, 81.0, -1.0, 0.0]],
        "0,157": [[0.9163519770340932, 0.0, 0.0, 81.0, 82.0, -1.0, 0.0]],
        "0,158": [[0.9173039526080974, 0.0, 0.0, 82.0, 83.0, -1.0, 0.0]],
        "0,159": [[0.9175119130111778, 0.0, 0.0, 82.0, 83.0, -1.0, 0.0]],
        "0,160": [[0.9194428358580002, 0.0, 0.0, 83.0, 84.0, -1.0, 0.0]],
        "0,161": [[0.924932069814417, 0.0, 0.0, 84.0, 85.0, -1.0, 0.0]],
        "0,162": [[0.9250911041942417, 0.0, 0.0, 85.0, 86.0, -1.0, 0.0]],
        "0,163": [[0.9258621837279098, 0.0, 0.0, 86.0, 87.0, -1.0, 0.0]],
        "0,164": [[0.9294120873157172, 0.0, 0.0, 86.0, 87.0, -1.0, 0.0]],
        "0,165": [[0.9317447321592218, 0.0, 0.0, 87.0, 88.0, -1.0, 0.0]],
        "0,166": [[0.9359727568391324, 0.0, 0.0, 88.0, 89.0, -1.0, 0.0]],
        "0,167": [[0.9382517062549935, 0.0, 0.0, 88.0, 89.0, -1.0, 0.0]],
        "0,168": [[0.939013687515452, 0.0, 0.0, 89.0, 90.0, -1.0, 0.0]],
        "0,169": [[0.9401569660416313, 0.0, 0.0, 90.0, 91.0, -1.0, 0.0]],
        "0,170": [[0.9484156655954205, 0.0, 0.0, 90.0, 91.0, -1.0, 0.0]],
        "0,171": [[0.9501284980412695, 0.0, 0.0, 91.0, 92.0, -1.0, 0.0]],
        "0,172": [[0.959265108523301, 0.0, 0.0, 91.0, 92.0, -1.0, 0.0]],
        "0,173": [[0.9648911695577099, 0.0, 0.0, 92.0, 93.0, -1.0, 0.0]],
        "0,174": [[0.9712201964016566, 0.0, 0.0, 92.0, 93.0, -1.0, 0.0]],
        "0,175": [[0.9718220090271169, 0.0, 0.0, 93.0, 94.0, -1.0, 0.0]],
        "0,176": [[0.9801453760377458, 0.0, 0.0, 93.0, 94.0, -1.0, 0.0]],
        "0,177": [[0.9803468350580087, 0.0, 0.0, 94.0, 95.0, -1.0, 0.0]],
        "0,178": [[0.9808950719771008, 0.0, 0.0, 95.0, 96.0, -1.0, 0.0]],
        "0,179": [[0.9810835602313082, 0.0, 0.0, 96.0, 97.0, -1.0, 0.0]],
        "0,180": [[0.9846244462956597, 0.0, 0.0, 97.0, 98.0, -1.0, 0.0]],
        "0,181": [[0.9885463152516017, 0.0, 0.0, 97.0, 98.0, -1.0, 0.0]],
        "0,182": [[0.992016531990525, 0.0, 0.0, 98.0, 99.0, -1.0, 0.0]],
        "0,183": [[0.9939416145822181, 0.0, 0.0, 99.0, 100.0, -1.0, 0.0]],
        "0,184": [[0.9942793740424737, 0.0, 0.0, 100.0, 101.0, -1.0, 0.0]],
        "0,185": [[0.9968934560679362, 0.0, 0.0, 101.0, 102.0, -1.0, 0.0]],
        "0,186": [[0.9971488574379493, 0.0, 0.0, 102.0, 103.0, -1.0, 0.0]],
        "0,187": [[0.9977390831641405, 0.0, 0.0, 102.0, 103.0, -1.0, 0.0]],
        "0,188": [[1.0044874720561072, 0.0, 0.0, 103.0, 104.0, -1.0, 0.0]]
    };
}

export const reducer = (state, action) => {
    state = state || initialState;
    console.log(`Smo log input action: ${action.type}`);
    console.log(state);

    if (action.type === setInitStateType) {
        return initialState;
    }
    
    if (action.type === getDataFromSockets) {
        let {channelCount, passengersOnBus, passengers} = action.payload;
        let startQuality = 0.7;
        let channelsQuality = [...Array(channelCount)].map((x, index) => Number((startQuality - 0.05 * index).toFixed(2)));
        let transformedData = transformData(passengers, channelsQuality);
        let smoResults = [];
        let set = new Set();
        for (let item of transformedData) {
            if (!set.has(item.agentId)) {
                set.add(item.agentId);
                smoResults.push(item);
            }
        }
        console.log(state);
        return {
            ...state,
            isInitState: false,
            smoResults: smoResults.slice(0, passengersOnBus),
            isInteractiveMode: false
        }
    }

    if (action.type === setSmoInteractiveMode) {
        return {
            ...state,
            isInteractiveMode: true,
            smoSteps: [...state.smoSteps, action.payload]
        }
    }
    
    if (action.type === runNextStepForSMO) {
        return {
            ...state,
            smoSteps: [...state.smoSteps, action.payload]
        }
    }

    return state;
};

