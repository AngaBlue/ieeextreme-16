"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const process_1 = require("process");
const readline_1 = require("readline");
const rl = (0, readline_1.createInterface)({ input: process_1.stdin, output: process_1.stdout });
const iterator = rl[Symbol.asyncIterator]();
async function getLine() {
    const line = await iterator.next();
    return line.value;
}
function indexOfSmallest(a) {
    let lowest = 0;
    for (let i = 1; i < a.length; i++) {
        if (a[i] < a[lowest])
            lowest = i;
    }
    return lowest;
}
async function scheduler() {
    const base = 1n;
    const [_, workerCount] = (await getLine()).split(' ').map(Number);
    const tasks = (await getLine()).split(' ').map(t => Number(t)).sort((a, b) => b - a).map(t => base << BigInt(t));
    const workers = new Array(workerCount).fill(0n);
    for (let i = 0; i < tasks.length; i++) {
        const worker = indexOfSmallest(workers);
        workers[worker] += tasks[i];
    }
    let largest = 0n;
    for (let i = 0; i < workers.length; i++) {
        if (workers[i] > largest)
            largest = workers[i];
    }
    console.log((largest % 1000000007n).toString());
}
scheduler();
