#include <stdint.h>
#include <iostream>
#include <vector>
#include <algorithm>

typedef std::vector<uint_fast16_t> tasks_t;

const uint_fast64_t modulo = 10e9 + 7;
const uint_fast16_t base = 2;

uint_fast64_t mod_pow(uint_fast64_t x, uint_fast64_t y, uint_fast64_t p)
{
    uint_fast64_t res = 1; // Initialize result

    x = x % p; // Update x if it is more than or
               // equal to p

    if (x == 0)
        return 0; // In case x is divisible by p;

    while (y > 0)
    {
        // If y is odd, multiply x with result
        if (y & 1)
            res = (res * x) % p;

        // y must be even now
        y = y >> 1; // y = y/2
        x = (x * x) % p;
    }
    return res;
}

int main(int argc, char const *argv[])
{
    // Get input
    uint_fast16_t task_count, worker_count;
    std::cin >> task_count >> worker_count;

    // Get tasks
    auto tasks = std::vector<uint_fast16_t>(task_count);
    for (auto &task : tasks)
        std::cin >> task;

    // Sort tasks
    std::sort(tasks.begin(), tasks.end(), std::less<uint_fast16_t>());

    // Create workers
    std::vector<tasks_t> worker_tasks;
    worker_tasks.assign(worker_count, tasks_t());

    // Assign tasks
    for (auto &task : tasks)
    {
        // Find worker with smallest value in 0th position
        tasks_t least_work = worker_tasks.front();
        for (auto &worker : worker_tasks)
        {
            if (worker.size() == 0) {
                least_work = worker;
                break;
            }
            if (worker.front() < least_work.front())
            {
                least_work = worker;
            }
        }

        // Find where to insert task
        if (least_work.size() == 0)
        {
            least_work.push_back(task);
            std::cout << least_work.front() << std::endl;
        }
        else
        {
            uint_fast16_t task_time = task;
            size_t task_position = least_work.size() - 1;
            while (task_time != least_work[task_position])
            {
                task_time++;
                if (task_position == 0)
                    break;
                task_position--;
            }

            // Clamp to least work to task position & append
            least_work.resize(task_position);
            least_work.push_back(task);
        }
    }

    // Find max worker time
    tasks_t most_work = worker_tasks[0];
    for (auto &worker : worker_tasks)
    {
        if (worker[0] < most_work[0])
        {
            most_work = worker;
        }
    }

    // Calculate total work with modulus
    uint_fast64_t output = 0;
    for (auto &task : most_work)
    {
        output %= modulo;
        output += mod_pow(base, task, modulo);
    }

    std::cout << output << std::endl;

    return 0;
}
