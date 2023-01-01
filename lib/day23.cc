#include <iostream>
#include <vector>
#include <string>

using namespace std;

bool step(vector<int> &work, vector<int> &tmp, vector<int> &scratch, int round) {
    const static int moves[4][2] = { {0, -1}, {0, 1}, {-1, 0}, {1, 0} };
    const static int scan[8] = { -257, -256, -255, 1, 257, 256, 255, -1 };
    const static int move2scan[4] = { 0, 4, 6, 2 };
    int cnt[9] = { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    bool moved = false;
    for (int p = 0; p < work.size(); p++) { // part 1
        int code = work[p], x = code & 0xFF, nx = x, y = code >> 8, ny = y, tot = 0;
        for (int s = 0; s < 8; s++) {
            cnt[s] = ((scratch[code + scan[s]] & 0xFFFF) == round) ? 1 : 0;
            tot += cnt[s];
        }
        if (tot != 0) {
            cnt[8] = cnt[0];
            for (int i = 0; i < 4; i++) {
                int idx = (i + round) % 4;
                int spos = move2scan[idx];
                tot = cnt[spos] + cnt[spos + 1] + cnt[spos + 2];
                if (tot == 0) {
                    nx = x + moves[idx][0];
                    ny = y + moves[idx][1];
                    break;
                }
            }
            moved = true;
        }
        tmp[p] = (ny << 8) + nx;
        scratch[tmp[p]] += 1 << 16;
    }
    for (int i = 0; i < work.size(); i++) {
        int ncode = tmp[i], nx = ncode & 0xFF, ny = ncode >> 8;
        if (scratch[ncode] >> 16 == 1) {
            scratch[work[i]] = 0;
            scratch[ncode] = round + 1;
            work[i] = ncode;
        } else {
            scratch[ncode] = 0;
            scratch[work[i]] = round + 1;
        }
    }
    return moved;
}


int main() {
    string s;
    vector<string> input;
    ios::sync_with_stdio(false);
    while (cin >> s) input.push_back(s);
    for (int i = 0; i < 100; i++) {
        vector<int> data, scratch, tmp;
        scratch.resize(input.size()*256*2);
        fill(scratch.begin(), scratch.end(), 0);
        for (int y = 0; y < input.size(); y++) {
            for (int x = 0; x < input[y].length(); x++) {
                int code = ((y + 16) << 8) + (x + 16);
                if (input[y][x] == '#') {
                    data.push_back(code);
                    scratch[code]=2000;
                }
            }
        }
        tmp.resize(data.size());
        int round = 0;
        while (step(data, tmp, scratch, 2000 + round)) round++;
        cout << round << endl;
    }
    return 0;
}
