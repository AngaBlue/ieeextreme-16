import { stdin, stdout } from 'process';
import { createInterface } from 'readline';

const rl = createInterface({ input: stdin, output: stdout });
const iterator = rl[Symbol.asyncIterator]();
async function getLine(): Promise<string> {
    const line = await iterator.next();
    return line.value;
}

async function myTreat() {
    // Map of people to dinners received
    const received: Record<string, number> = {};
    const given: Record<string, number> = {};
    const people = Number(await getLine());
    const names = new Set<string>();

    // Read all records
    for (let i = 0; i < people; i++) {
        const [giver, count, ...recipients] = (await getLine()).split(' ');

        // Add to received
        recipients.forEach(recipient => {
            names.add(recipient);
            received[recipient] = (received[recipient] ?? 0) + 1;
        });

        // Add to given
        names.add(giver);
        given[giver] = (given[giver] ?? 0) + Number(count);
    }

    // Most and least dinners received
    const outstanding = [...names].map(name => {
        const differential = (given[name] ?? 0) - (received[name] ?? 0);
        return differential > 0 ? differential : 0;
    })

    const largestDifference = Math.max(...outstanding);
    const total = outstanding.reduce((a, b) => a + b, 0);

    console.log(total, largestDifference);
}


async function main() {
    // Number of test cases
    const cases = Number(await getLine());

    for (let i = 0; i < cases; i++) {
        await myTreat();
    }
}

main();
