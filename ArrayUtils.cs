namespace FuiEditor
{
    static class ArrayUtils
    {
        //https://teratail.com/questions/20408
        //author: yuba, thx!

        /// <summary>
        /// byte配列の中からbyteパターンと一致する並びを検索する。
        /// アルゴリズムはSunday法。
        /// </summary>
        /// <param name="pattern">探すパターン</param>
        /// <param name="text">探索範囲となる配列</param>
        /// <returns>発見した場合は開始位置、発見できなかった場合は-1</returns>
        public static int SearchBytes(byte[] text, int startIndex, byte[] pattern)
        {
            int patternLen = pattern.Length, textLen = text.Length;

            // 移動量テーブルの作成
            int[] qs_table = new int[byte.MaxValue + 1];

            // デフォルト（パターン中に存在しないキャラクタが比較範囲の直後にあった）の場合、
            // 次の比較範囲はそのキャラクタの次。（＝比較範囲ずらし幅はパターン長＋１）
            for (int i = qs_table.Length; i-- > 0;) qs_table[i] = patternLen + 1;

            // パターンに存在するキャラクタが比較範囲の直後にあった場合、
            // 次の比較範囲は、そのキャラクタとパターン中のキャラクタを一致させる位置に。
            for (int n = 0; n < patternLen; ++n) qs_table[pattern[n]] = patternLen - n;

            int pos;

            // 移動量テーブルを用いて、文章の末尾に達しない範囲で比較を繰り返す
            for (pos = startIndex; pos < textLen - patternLen; pos += qs_table[text[pos + patternLen]])
            {
                // 一致するか比較。一致したら、そのときの比較位置を返す。
                if (CompareBytes(text, pos, pattern, patternLen)) return pos;
            }

            // 文章の末尾がまだ未比較なら、そこも比較しておく
            if (pos == textLen - patternLen)
            {
                // 一致するか比較。一致したら、そのときの比較位置を返す。
                if (CompareBytes(text, pos, pattern, patternLen)) return pos;
            }

            // 一致する位置はなかった。
            return -1;
        }

        /// <summary>
        /// 配列(pattern)が別の配列(text)に含まれているかを判定する。
        /// 
        /// pos + patternLen が text.Length より大きかったり
        /// pos や patternLen が 0 未満だったり、
        /// needdleLen が pattern.Length より大きかったりすると
        /// ArrayOutOfBoundException が発生する。
        /// </summary>
        /// <param name="text">この配列の pos 番目からを pattern と比較する</param>
        /// <param name="pos">text のどこから比較するか</param>
        /// <param name="pattern">この配列全体が、text の pos 番目からと一致しているかを判定する</param>
        /// <param name="patternLen">patternのうち一致判定する長さ</param>
        /// <returns></returns>
        static bool CompareBytes(byte[] text, int pos, byte[] pattern, int patternLen)
        {
            for (int comparer = 0; comparer < patternLen; ++comparer)
            {
                if (text[comparer + pos] != pattern[comparer]) return false;
            }
            return true;
        }
    }
}
