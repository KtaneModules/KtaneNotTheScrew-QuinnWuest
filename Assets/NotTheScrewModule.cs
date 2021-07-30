using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using UnityEngine;

using Rnd = UnityEngine.Random;

public class NotTheScrewModule : MonoBehaviour
{
    public KMAudio Audio;
    public KMBombModule Module;
    public KMBombInfo Info;
    public KMRuleSeedable RuleSeedable;

    public KMSelectable[] holes;
    public KMSelectable[] buttons;
    public GameObject screw;
    public MeshRenderer[] outlines;
    public TextMesh[] buttonText;
    public TextMesh screenText;
    public Texture[] outlineTexture;

    private static int _moduleIdCounter = 1;
    private int _moduleId = 0;

    private int screwLoc;
    private readonly float[] holeXPos = { -0.06f, 0f, 0.06f, -0.06f, 0f, 0.06f };
    private readonly float[] holeZPos = { -0.02f, -0.02f, -0.02f, -0.06f, -0.06f, -0.06f };
    private bool _moduleSolved = false;
    private bool _screwInsert = true;
    private bool _coroutineRunning = false;

    private static readonly int[][] allSudokus = "123421341243214312432134,123423341241234112412334,123441341223412312234134,123443341221432112214334,123421342143214312431234,123443342121431212214334,123421431234214312342143,123434431221342112214343,123421432134214312341243,123424432131241312314243,123431432124314212241343,123434432121341212214343,124321341243213412432134,124343341221432112213434,124321342143213412431234,124323342141231412413234,124341342123413212231434,124343342121431212213434,124321431234213412342143,124324431231243112312443,124331431224312412243143,124334431221342112213443,124321432134213412341243,124334432121341212213443,132431241342314213423124,132432241341324113413224,132441241332413213324124,132442241331423113314224,132431243142314213421324,132442243131421313314224,132424421331243113314242,132431421324314213243142,132421423134214313341242,132424423131241313314242,132431423124314213241342,132434423121341213214342,134231241342312413423124,134242241331423113312424,134231243142312413421324,134232243141321413412324,134241243132412313321424,134242243131421313312424,134221421334213413342142,134224421331243113312442,134231421324312413243142,134234421321342113213442,134224423131241313312442,134231423124312413241342,142331231442314214423123,142332231441324114413223,142341231432413214324123,142342231431423114314223,142332234141321414413223,142341234132413214321423,142323321441234114413232,142341321423413214234132,142321324143213414431232,142323324141231414413232,142341324123413214231432,142343324121431214213432,143232231441324114412323,143241231432412314324123,143231234142312414421323,143232234141321414412323,143241234132412314321423,143242234131421314312423,143221321443214314432132,143223321441234114412332,143241321423412314234132,143243321421432114214332,143223324141231414412332,143241324123412314231432,213412341243124321432134,213443341212432121124334,213412342143124321431234,213413342142134221421334,213442342113421321134234,213443342112431221124334,213412431234124321342143,213414431232142321324143,213432431214324121142343,213434431212342121124343,213412432134124321341243,213434432112341221124343,214312341243123421432134,214313341242132421423134,214342341213423121132434,214343341212432121123434,214312342143123421431234,214343342112431221123434,214312431234123421342143,214334431212342121123443,214312432134123421341243,214314432132143221321443,214332432114321421143243,214334432112341221123443,231431142342314223423114,231432142341324123413214,231441142332413223324114,231442142331423123314214,231432143241324123412314,231441143232412323324114,231414412332143223324141,231432412314324123143241,231412413234124323342141,231414413232142323324141,231432413214324123142341,231434413212342123124341,234132142341321423413214,234141142332413223321414,234131143242312423421314,234132143241321423412314,234141143232412323321414,234142143231421323312414,234112412334123423341241,234114412332143223321441,234132412314321423143241,234134412312341223123441,234114413232142323321441,234132413214321423142341,241331132442314224423113,241332132441324124413213,241341132432413224324113,241342132431423124314213,241331134242312424423113,241342134231423124312413,241313312442134224423131,241342312413423124134231,241312314243123424432131,241313314242132424423131,241342314213423124132431,241343314212432124123431,243131132442314224421313,243142132431421324314213,243131134242312424421313,243132134241321424412313,243141134232412324321413,243142134231421324312413,243112312443124324431231,243113312442134224421331,243142312413421324134231,243143312412431224124331,243113314242132424421331,243142314213421324132431,312413241342134231423124,312442241313423131134224,312412243143124331431224,312413243142134231421324,312442243113421331134224,312443243112431231124324,312413421324134231243142,312414421323143231234142,312423421314234131143242,312424421313243131134242,312413423124134231241342,312424423113241331134242,314212241343123431432124,314213241342132431423124,314242241313423131132424,314243241312432131123424,314213243142132431421324,314242243113421331132424,314213421324132431243142,314224421313243131132442,314213423124132431241342,314214423123142331231442,314223423114231431142342,314224423113241331132442,321423142341234132413214,321441142323413232234114,321421143243214332432114,321423143241234132412314,321441143223412332234114,321443143221432132214314,321413412324134232243141,321414412323143232234141,321423412314234132143241,321424412313243132134241,321414413223142332234141,321423413214234132142341,324121142343213432431214,324123142341231432413214,324141142323413232231414,324143142321431232213414,324123143241231432412314,324141143223412332231414,324114412323143232231441,324123412314231432143241,324113413224132432241341,324114413223142332231441,324123413214231432142341,324124413213241332132441,341221123443214334432112,341223123441234134412312,341241123423412334234112,341243123421432134214312,341221124343213434432112,341243124321432134213412,341212213443124334432121,341243213412432134124321,341212214343123434432121,341213214342132434423121,341242214313423134132421,341243214312432134123421,342121123443214334431212,342143123421431234214312,342121124343213434431212,342123124341231434413212,342141124323413234231412,342143124321431234213412,342112213443124334431221,342113213442134234421321,342142213413421334134221,342143213412431234124321,342112214343123434431221,342143214312431234123421,412314231432143241324123,412332231414324141143223,412312234134123441341223,412314234132143241321423,412332234114321441143223,412334234112341241123423,412313321424134241243132,412314321423143241234132,412323321414234141143232,412324321413243141134232,412314324123143241231432,412323324114231441143232,413212231434124341342123,413214231432142341324123,413232231414324141142323,413234231412342141124323,413214234132142341321423,413232234114321441142323,413214321423142341234132,413223321414234141142332,413213324124132441241332,413214324123142341231432,413223324114231441142332,413224324113241341132432,421324132431243142314213,421331132424314242243113,421321134234213442342113,421324134231243142312413,421331134224312442243113,421334134221342142213413,421313312424134242243131,421314312423143242234131,421323312414234142143231,421324312413243142134231,421313314224132442243131,421324314213243142132431,423121132434214342341213,423124132431241342314213,423131132424314242241313,423134132421341242214313,423124134231241342312413,423131134224312442241313,423113312424134242241331,423124312413241342134231,423113314224132442241331,423114314223142342231431,423123314214231442142331,423124314213241342132431,431221123434214343342112,431234123421342143214312,431221124334213443342112,431224124331243143312412,431231124324312443243112,431234124321342143213412,431212213434124343342121,431214213432142343324121,431232213414324143142321,431234213412342143124321,431212214334123443342121,431234214312342143123421,432121123434214343341212,432124123431241343314212,432131123424314243241312,432134123421341243214312,432121124334213443341212,432134124321341243213412,432112213434124343341221,432134213412341243124321,432112214334123443341221,432114214332143243321421,432132214314321443143221,432134214312341243123421"
                .Split(',').Select(str => str.Select(ch => ch - '1').ToArray()).ToArray();
    private int[] cells;
    private edgeInfo[][] squares;

    private int curPos;
    private int endPos;
    private int prevPos;
    private int[] holeColors;
    private string[] colors = { "blue", "green", "magenta", "red", "white", "yellow" };
    private List<int> labels;

    private bool[] passedThroughNums = new bool[4];
    private bool[] passedThroughLets = new bool[4];
    private bool[] passedThroughColors = new bool[6];

    class edgeInfo
    {
        public int color;
        public int letter;

        public override string ToString()
        {
            return string.Format("color: {0}, letter: {1}", color, letter);
        }
    }

    void Start()
    {
        _moduleId = _moduleIdCounter++;
        PlaceScrew();

        // RULE SEED

        var rnd = RuleSeedable.GetRNG();
        Debug.LogFormat("[Not The Screw #{0}] Using rule seed: {1}", _moduleId, rnd.Seed);

        rnd.Next(0, 2);

        squares = new edgeInfo[24][];
        for (var sqIx = 0; sqIx < squares.Length; sqIx++)
        {
            edgeInfo[] edges = new edgeInfo[4];
            int[] letters = { 0, 1, 2, 3 };
            rnd.ShuffleFisherYates(letters);
            for (var edIx = 0; edIx < edges.Length; edIx++)
            {
                if ((edIx == 0 && sqIx % 6 == 5) || (edIx == 1 && sqIx >= 18) || (edIx == 2 && sqIx % 6 == 0) || (edIx == 3 && sqIx < 6))
                    continue;
                List<int> usedCol = new List<int>();
                for (int ed2Ix = 0; ed2Ix < edIx; ed2Ix++)
                    if (edges[ed2Ix] != null)
                        usedCol.Add(edges[ed2Ix].color);
                if (edIx == 2 && sqIx % 6 != 0)
                    usedCol.Add(squares[sqIx - 1][0].color);
                if (edIx == 3 && sqIx >= 6)
                    usedCol.Add(squares[sqIx - 6][1].color);
                List<int> availCol = new List<int>();
                for (int col = 0; col < 6; col++)
                    if (!usedCol.Contains(col))
                        availCol.Add(col);
                int rand = rnd.Next(0, availCol.Count);
                edges[edIx] = new edgeInfo { color = availCol[rand], letter = letters[edIx] };
            }
            squares[sqIx] = edges;
        }

        cells = allSudokus[rnd.Next(0, allSudokus.Length)];

        // END RULE SEED

        screenText.text = "?";
        GeneratePositions();
    }

    private void Awake()
    {
        for (int i = 0; i < 4; i++)
        {
            int j = i;
            buttons[i].OnInteract += delegate ()
            {
                ButtonPress(j);
                return false;
            };
        }
        for (int i = 0; i < 6; i++)
        {
            int j = i;
            holes[i].OnInteract += delegate ()
            {
                HandleScrew(j);
                return false;
            };
        }
    }

    //change thijs
    void GeneratePositions()
    {
        //End position
        tryAgainEnd:
        holeColors = Enumerable.Range(0, 6).ToArray().Shuffle();
        var col = Array.IndexOf(holeColors, 3);
        var row = Array.IndexOf(holeColors, 1);
        if (row > col)
            row--;
        if (row == 4)
            goto tryAgainEnd;
        for (int i = 0; i < holes.Length; i++)
            outlines[i].material.mainTexture = outlineTexture[holeColors[i]];
        endPos = row * 6 + col;

        //Start position
        tryAgainStart:
        curPos = Rnd.Range(0, 24);
        if (curPos == endPos)
            goto tryAgainStart;

        passedThroughNums[cells[curPos]] = true;
        Debug.LogFormat("[Not The Screw #{0}] Starting position is column {1}, row {2}", _moduleId, (curPos % 6) + 1, (curPos / 6) + 1);
        Debug.LogFormat("[Not The Screw #{0}] Ending position is column {1}, row {2}", _moduleId, (endPos % 6) + 1, (endPos / 6) + 1);

        SetLabels();
    }

    private void SetLabels()
    {
        labels = new List<int>();
        labels.Add(3);
        labels.Insert((curPos & 1) != 0 ? 1 : 0, 2);
        labels.Insert((curPos / 2) % 3, 1);
        labels.Insert(curPos / 6, 0);
        for (int i = 0; i < 4; i++)
            buttonText[i].text = "ABCD".Substring(labels[i], 1);
    }

    void PlaceScrew()
    {
        screwLoc = Rnd.Range(0, 6);
        screw.transform.localPosition = new Vector3(holeXPos[screwLoc], -0.021f, holeZPos[screwLoc]);
    }

    void ButtonPress(int button)
    {
        Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, buttons[button].transform);
        buttons[button].AddInteractionPunch(0.5f);
        if (!_moduleSolved)
        {
            int index = squares[curPos].IndexOf(edge => edge != null && edge.letter == labels[button] && edge.color == holeColors[screwLoc]);
            if (index == -1)
            {
                Module.HandleStrike();
                Debug.LogFormat("[Not The Screw #{0}] Attempted to move from column {1}, row {2} through color {3} label {4}, which doesn't exist. Strike.", _moduleId, (curPos % 6) + 1, (curPos / 6) + 1, colors[holeColors[screwLoc]], "ABCD".Substring(labels[button], 1));
            }
            else
            {
                int temp = curPos;
                temp += (index == 0 ? 1 : index == 1 ? 6 : index == 2 ? -1 : -6);
                if (temp == prevPos)
                {
                    Module.HandleStrike();
                    Debug.LogFormat("[Not The Screw #{0}] Attempted to move from column {1}, row {2} to column {3}, row {4}, which you just previously travelled from. Strike.", _moduleId, (curPos % 6) + 1, (curPos / 6) + 1, (prevPos % 6) + 1, (prevPos / 6) + 1);
                }
                else
                {
                    prevPos = curPos;
                    curPos = temp;
                    screenText.text = (cells[curPos] + 1).ToString();
                    Debug.LogFormat("[Not The Screw #{0}] Moved to colum {1}, row {2}, with grid number {3}, passing through color {4}, letter {5}", _moduleId, (curPos % 6) + 1, (curPos / 6) + 1, cells[curPos] + 1, colors[holeColors[screwLoc]], "ABCD".Substring(labels[button], 1));
                    passedThroughNums[cells[curPos]] = true;
                    passedThroughColors[holeColors[screwLoc]] = true;
                    passedThroughLets[labels[button]] = true;
                    var numsDone = !passedThroughNums.Contains(false);
                    var letsDone = !passedThroughLets.Contains(false);
                    var colorsDone = !passedThroughColors.Contains(false);
                    if (curPos == endPos && (!numsDone || !letsDone || !colorsDone))
                    {
                        Module.HandleStrike();
                        if (!numsDone)
                            Debug.LogFormat("[Not The Screw #{0}] Did not pass through numbers: {1}", _moduleId, Enumerable.Range(0, 4).Where(i => !passedThroughNums[i]).Select(i => i + 1).Join(", "));
                        if (!letsDone)
                            Debug.LogFormat("[Not The Screw #{0}] Did not pass through letters: {1}", _moduleId, Enumerable.Range(0, 4).Where(i => !passedThroughLets[i]).Select(i => "ABCD"[i]).Join(", "));
                        if (!colorsDone)
                            Debug.LogFormat("[Not The Screw #{0}] Did not pass through colors: {1}", _moduleId, Enumerable.Range(0, 6).Where(i => !passedThroughColors[i]).Select(i => colors[i]).Join(", "));
                        Debug.LogFormat("[Not The Screw #{0}] Attempted to move to the ending square, but not all requirements have been completed. Strike.", _moduleId);
                    }
                    else if (curPos == endPos && numsDone && letsDone && colorsDone)
                    {
                        _moduleSolved = true;
                        Module.HandlePass();
                        Debug.LogFormat("[Not The Screw #{0}] Travelled to the ending square after completing all requirements. Module solved!", _moduleId);
                    }
                }
                SetLabels();
            }
        }
    }

    void HandleScrew(int n)
    {
        //unscrew
        if (_screwInsert == true && n == screwLoc && !_coroutineRunning && !_moduleSolved)
        {
            Audio.PlaySoundAtTransform("screwdriver_sound", holes[n].transform);
            StartCoroutine(AnimateScrew(screwLoc, screwIn: false));
            if (!_moduleSolved)
                screenText.text = "";
        }

        //screw
        if (_screwInsert == false && !_coroutineRunning && !_moduleSolved)
        {
            screwLoc = n;
            Audio.PlaySoundAtTransform("screwdriver_sound", holes[n].transform);
            StartCoroutine(AnimateScrew(screwLoc, screwIn: true));
            Debug.LogFormat("[Screw #{0}] Screw in to hole {1}", _moduleId, screwLoc + 1);
            if (!_moduleSolved)
                screenText.text = "?";
        }
    }

    IEnumerator AnimateScrew(int screwIx, bool screwIn)
    {
        _coroutineRunning = true;
        screw.SetActive(true);

        var duration = .5f;
        var elapsed = 0f;
        while (elapsed < duration)
        {
            screw.transform.localPosition = new Vector3(holeXPos[screwIx], Mathf.Lerp(screwIn ? .021f : -.021f, screwIn ? -.021f : .021f, elapsed / duration), holeZPos[screwIx]);
            screw.transform.localEulerAngles = new Vector3(0, (screwIn ? (1 - elapsed / duration) : (elapsed / duration)) * -360f, 0);
            yield return null;
            elapsed += Time.deltaTime;
        }
        screw.transform.localPosition = new Vector3(holeXPos[screwIx], screwIn ? -.021f : .021f, holeZPos[screwIx]);
        screw.transform.localEulerAngles = new Vector3(0, (screwIn ? 0 : -360f), 0);

        if (!screwIn)
            screw.SetActive(false);

        _coroutineRunning = false;
        _screwInsert = screwIn;
    }
#pragma warning disable 414
    private readonly string TwitchHelpMessage = @"Unscrew with “!{0} unscrew”. Put the screw in the red hole with “!{0} screw R”. Press a button with “!{0} press A” (label). Commands can be chained with commas. The screw will be unscrewed before screwing it into a hole.";
#pragma warning restore 414

    IEnumerator ProcessTwitchCommand(string command)
    {
        string[] commands = command.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(x =>
            x.Trim().ToUpperInvariant()).ToArray();
        List<Match[]> matches = new List<Match[]>();
        foreach (string cmd in commands)
            matches.Add(new Match[]
            {
                Regex.Match(cmd, @"^UNSCREW$"),
                Regex.Match(cmd, @"^SCREW\s+([BGMRWY])$"),
                Regex.Match(cmd, @"^PRESS\s+([ABCD])$")
            });
        for (int i = 0; i < commands.Length; i++)
            if (matches[i].All(x => !x.Success))
            {
                yield return "sendtochaterror Invalid command at position " + (i + 1);
                yield break;
            }
        yield return null;
        foreach (Match[] matchGroup in matches)
        {
            if (matchGroup[0].Success && _screwInsert)
            {
                holes[screwLoc].OnInteract();
                yield return new WaitUntil(() => !_coroutineRunning);
            }
            else if (matchGroup[1].Success)
            {
                if (_screwInsert)
                {
                    holes[screwLoc].OnInteract();
                    yield return new WaitUntil(() => !_coroutineRunning);
                }
                int submitCol = "BGMRWY".IndexOf(matchGroup[1].Groups[1].Value[0]);
                holes[Array.IndexOf(holeColors, submitCol)].OnInteract();
                yield return new WaitUntil(() => !_coroutineRunning);
            }
            else if (matchGroup[2].Success)
            {
                int submitButton = matchGroup[2].Groups[1].Value[0] - 'A';
                buttons[Array.IndexOf(labels.ToArray(), submitButton)].OnInteract();
                yield return new WaitForSeconds(0.2f);
            }
        }

    }

    sealed class DijkstraNode : IEquatable<DijkstraNode>
    {
        public bool[] passedThroughNums;
        public bool[] passedThroughLets;
        public bool[] passedThroughColors;
        public int cell;
        public int prevCell;

        public bool Equals(DijkstraNode other)
        {
            var otherNode = other as DijkstraNode;
            return otherNode != null
                && otherNode.cell == cell
                && otherNode.passedThroughNums.SequenceEqual(passedThroughNums)
                && otherNode.passedThroughLets.SequenceEqual(passedThroughLets)
                && otherNode.passedThroughColors.SequenceEqual(passedThroughColors);
        }
        public override bool Equals(object obj)
        {
            return Equals(obj as DijkstraNode);
        }
        public override int GetHashCode()
        {
            var i = cell;
            i = passedThroughNums.Aggregate(i, (p, n) => unchecked((p << 1) | (n ? 1 : 0)));
            i = passedThroughLets.Aggregate(i, (p, n) => unchecked((p << 1) | (n ? 1 : 0)));
            i = passedThroughColors.Aggregate(i, (p, n) => unchecked((p << 1) | (n ? 1 : 0)));
            return i;
        }
    }

    struct QueueItem
    {
        public DijkstraNode Node;
        public DijkstraNode Parent;
    }

    IEnumerator TwitchHandleForcedSolve()
    {
        if (_moduleSolved)
            yield break;

        var q = new Queue<QueueItem>();
        var visited = new Dictionary<DijkstraNode, DijkstraNode>();

        var startNode = new DijkstraNode
        {
            cell = curPos,
            passedThroughColors = passedThroughColors,
            passedThroughLets = passedThroughLets,
            passedThroughNums = passedThroughNums
        };

        DijkstraNode goalNode = null;

        q.Enqueue(new QueueItem { Node = startNode, Parent = null });
        while (q.Count > 0)
        {
            var item = q.Dequeue();
            if (visited.ContainsKey(item.Node))
                continue;
            visited[item.Node] = item.Parent;

            // Found the goal?
            if (item.Node.cell == endPos && !item.Node.passedThroughLets.Contains(false) && !item.Node.passedThroughColors.Contains(false) && !item.Node.passedThroughNums.Contains(false))
            {
                goalNode = item.Node;
                break;
            }
            // If we’ve not fulfilled the requirements, we can’t visit the goal node
            if (item.Node.cell == endPos)
                continue;

            for (var edIx = 0; edIx < 4; edIx++)
            {
                if (squares[item.Node.cell][edIx] == null)
                    continue;

                var newCell = item.Node.cell + (edIx == 0 ? 1 : edIx == 1 ? 6 : edIx == 2 ? -1 : -6);
                if (newCell == item.Node.prevCell)
                    continue;

                var newPassedThroughColors = item.Node.passedThroughColors.ToArray();
                newPassedThroughColors[squares[item.Node.cell][edIx].color] = true;

                var newPassedThroughLets = item.Node.passedThroughLets.ToArray();
                newPassedThroughLets[squares[item.Node.cell][edIx].letter] = true;

                var newPassedThroughNums = item.Node.passedThroughNums.ToArray();
                newPassedThroughNums[cells[newCell]] = true;

                q.Enqueue(new QueueItem
                {
                    Parent = item.Node,
                    Node = new DijkstraNode
                    {
                        cell = newCell,
                        prevCell = item.Node.cell,
                        passedThroughColors = newPassedThroughColors,
                        passedThroughLets = newPassedThroughLets,
                        passedThroughNums = newPassedThroughNums
                    }
                });
            }
        }

        if (goalNode == null)
        {
            Debug.LogFormat("[Not The Screw #{0}] The autosolver did not find a strikeless path to the goal. Did you navigate into a corner where a strike is unavoidable?", _moduleId);
            yield break;
        }

        // Reconstruct the path to the solution
        var curNode = goalNode;
        var steps = new List<int>();    // edge index we need to traverse at each step
        while (curNode != null)
        {
            var newNode = visited[curNode];
            if (newNode == null)
                break;

            var cellDiff = curNode.cell - newNode.cell;
            steps.Add(cellDiff == 1 ? 0 : cellDiff == 6 ? 1 : cellDiff == -1 ? 2 : 3);
            curNode = newNode;
        }

        // Push the relevant buttons
        for (int i = steps.Count - 1; i >= 0; i--)
        {
            var edge = squares[curPos][steps[i]];
            var desiredHole = Array.IndexOf(holeColors, edge.color);
            if (screwLoc != desiredHole)
            {
                if (_screwInsert)
                {
                    holes[screwLoc].OnInteract();
                    while (_coroutineRunning)
                        yield return true;
                }
                holes[desiredHole].OnInteract();
                while (_coroutineRunning)
                    yield return true;
            }

            buttons[labels.IndexOf(edge.letter)].OnInteract();
            yield return new WaitForSeconds(.1f);
        }
    }
}