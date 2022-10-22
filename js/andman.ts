import { stdin, stdout } from 'process';
import { createInterface } from 'readline';

class TreeNode {
    parent?: TreeNode;

    index: number;

    weight = 1;

    constructor(index: number) {
        this.index = index;
    }
}

const rl = createInterface({ input: stdin, output: stdout });
const iterator = rl[Symbol.asyncIterator]();
async function getLine(): Promise<string> {
    const line = await iterator.next();
    return line.value;
}

function getPathFromRoot(node: TreeNode): TreeNode[] {
    const path: TreeNode[] = [node];
    while (path[0].parent) {
        path.unshift(path[0].parent);
    }
    return path;
}

async function andman() {
    // The amount of nodes in the tree
    const size = Number(await getLine());

    const nodes = new Array(size + 1).fill(1).map((_, i) => new TreeNode(i));

    // Get weight of each node
    (await getLine()).split(' ').map(Number).map((weight, k) => nodes[k + 1].weight = weight);

    for (let i = 0; i < size - 1; i++) {
        // Read node relation
        const [parentIndex, childIndex] = (await getLine()).split(' ').map(Number);
        const parent = nodes[parentIndex];
        const child = nodes[childIndex];

        // Set parent and child
        child.parent = parent;
    }

    // Get the number of operations
    const opCount = Number(await getLine());

    // Perform operations
    for (let i = 0; i < opCount; i++) {
        const [op, ...args] = (await getLine()).split(' ').map(Number);

        if (op === 2) {
            // Calculate product of weights
            const [startIndex, endIndex] = args;
            const start = nodes[startIndex];
            const end = nodes[endIndex];

            const startPath = getPathFromRoot(start);
            const endPath = getPathFromRoot(end);

            // Find where the paths diverge
            let divergeIndex = 0;
            for (let k = 0; k < startPath.length; k++) {
                divergeIndex = k;
                if (!endPath[k] || startPath[k] !== endPath[k]) {
                    break;
                }
            }

            const path = Array.from(new Set([...startPath.slice(divergeIndex - 1), ...endPath.slice(divergeIndex)]));

            const product = path.reduce((acc, node) => acc * BigInt(node.weight), BigInt(1));

            const modulo = BigInt(1000000007);
            console.log((product % modulo).toString());
        } else {
            // Change weight of node
            const [index, weight] = args;
            nodes[index].weight = weight;
        }
    }
}


async function main() {
    // Number of test cases
    const cases = Number(await getLine());

    for (let i = 0; i < cases; i++) {
        await andman();
    }
}

main();
