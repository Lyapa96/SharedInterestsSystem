import {expect} from "chai";
import {transformData} from './dataParser';

describe("parse data", function () {
    it("when one passenger have several trips", function () {
        let data = {
            "0,0": [
                [0.03484852846663591, 0.03484852846663591, 0.04484852846663591, 0.0, 1.0, 1.0, 0.0],
                [0.04484852846663591, 0.04484852846663591, 0.13753919107576162, 0.0, 1.0, 1.0, 1.0],
                [0.13753919107576162, 0.0, 0.0, 0.0, 0.0, -1.0, 21.0]]
        };

        let newData = transformData([data]);  
        
        expect(newData[0].quality).to.equal(1);
        expect(newData[1].quality).to.equal(2);
        expect(newData[2].quality).to.equal(3);
    });
});