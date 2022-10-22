const readline = require('node:readline/promises');
const { stdin, stdout } = require('node:process');

// Mappings
const handMap = {
    head: 1,
    hip: 2,
    start: 0
};

// Start readline
const rl = readline.createInterface({ input: stdin, output: stdout });

let tests = null, inputs = null, count = 0, testCount = 0;
let forwards = true;
let hands = [0, 0]; // 0 = start, 1 = head, 2 = hip
let legs = [0, 0]; // 0 = start, 1 = in

function printDancer() {
    // UPPER
    console.log(`${hands[0] === handMap.head ? '(' : ' '}o${hands[1] === handMap.head ? ')' : ' '}`);

    // TORSO
    let leftHand = ' ', rightHand = ' ';
    // Left
    if (hands[0] === handMap.hip) leftHand = '<';
    else if (hands[0] === handMap.start) leftHand = '/';

    // Right
    if (hands[1] === handMap.hip) rightHand = '>';
    else if (hands[1] === handMap.start) rightHand = '\\';

    console.log(`${leftHand}|${rightHand}`);

    // LOWER
    console.log(`${legs[0] ? '<' : '/'} ${legs[1] ? '>' : '\\'}`);
}

function goNext() {
    count = 0;
    inputs = null;
    hands = [0, 0];
    legs = [0, 0];

    if (tests === testCount) {
        process.exit(0);
    }
}

// Get the input
rl.on('line', line => {
    // Metadata
    if (tests === null) {
        tests = Number(line);
        if (tests === 0) process.exit(0);
        return;
    }
    if (inputs === null) {
        testCount++;
        inputs = Number(line);
        if (!inputs) goNext();
        return
    }

    count++;

    const args = line.split(' ');
    // Check if say
    switch (args[0]) {
        case 'say':
            const message = line.slice(4);
            console.log(message);
            break;
        case 'turn':
            hands = hands.reverse();
            legs = legs.reverse();
            forwards = !forwards;
            printDancer();
            break;
        default:
            direction = forwards ^ Number(args[0] !== 'left');
            // Check if hand or leg
            switch (args[1]) {
                case 'hand':
                    hands[direction] = handMap[args[3]];
                    break;
                case 'leg':
                    legs[direction] = args[2] === 'out' ? 0 : 1;
                    break;
            }
            printDancer();
    }

    // Check if end
    if (inputs === count) goNext();
});
