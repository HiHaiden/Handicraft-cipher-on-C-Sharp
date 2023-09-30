// Файл Program.cs:
/*
  * Рассмотрим следующую последовательность операций:
  * Открытый текст -> S1 -> P1 -> S2 -> P2 -> Зашифрованный текст
  * Открытым текстом является ASKORBINKA.
  * S1 — шифр сдвига с ключом kS1 = 17.
  * S2 — шифр сдвига с ключом kS2 = 8.
  * P1 — перестановочный шифр с ключом kP1 = (5, 1, 3, 2, 4).
  * P2 — перестановочный шифр с ключом kP2 = (3, 4, 5, 1, 2).
*/
using System;

namespace Decipher
{
    class Program
    {
        // Это объявление наших Букв Алфавита (в Алфавитном порядке).
        public static readonly char[] AlphabetLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

        static void Main()
        {
            // НАЧАЛО ОБЪЯВЛЕНИЯ
            // Для shiftOne и shiftTwo мы объявляем номер смены для наших шифров сдвига.
            int shiftOne = 17;
            int shiftTwo = 8;

            // Для permutationOne и permutationTwo мы объявляем ключи перестановки
            int[] permutationOne = { 5, 1, 3, 2, 4 };
            int[] permutationTwo = { 3, 4, 5, 1, 2 };

            // Наш открытый текст — это текст, который мы хотим зашифровать.
            string plaintext = "ASKORBINKA";

            // Делаю из массива открытого текста.
            char[] plaintextArray = plaintext.ToCharArray();
            // КОНЕЦ ОБЪЯВЛЕНИЯ

            // СТАРТ ЛОГИКИ
            char[] cipherOne = ShiftCipher(shiftOne, plaintextArray);
            char[] cipherTwo = PermutationCipher(permutationOne, cipherOne);
            char[] cipherThree = ShiftCipher(shiftTwo, cipherTwo);
            char[] cipherFour = PermutationCipher(permutationTwo, cipherThree);
            // КОНЕЦ ЛОГИКИ

            // ВЫВОД РЕЗУЛЬТАТОВ НАЧАЛО
            Console.WriteLine(plaintext + " - Перед шифрованием");
            Console.WriteLine(new string(cipherOne) + " - После первой фазы (сдвиг ключом kS1=17)");
            Console.WriteLine(new string(cipherTwo) + " - После второй фазы (перестановка с ключом kP1 = (5, 1, 3, 2, 4))");
            Console.WriteLine(new string(cipherThree) + " - После третьей фазы (сдвиг ключом kS2=8))");
            Console.WriteLine(new string(cipherFour) + " - После последней фазы (перестановка с ключом kP2 = (3, 4, 5, 1, 2))");
            Console.ReadLine();
            // ВЫВОД РЕЗУЛЬТАТОВ КОНЕЦ
        }

        // Это функция шифрования Shift Cipher.
        private static char[] ShiftCipher(int shiftCount, char[] plaintextArray)
        {
            // Сначала мы объявляем новый массив символов.
            // Он будет содержать результат нашего зашифрованного открытого текста.
            // Длина "зашифрованного" символа такая же, как и у открытого массива, потому что он должен содержать такое же количество букв, но со смещением.
            char[] encrypted = new char[plaintextArray.Length];

            // Мы перебираем весь текст.
            // (Так что работаем с каждой буквой по отдельности).
            for (int i = 0; i < plaintextArray.Length; i++)
            {
                // Мы ищем в 'AlphabetLetters' букву, которая равна нашей букве открытого текста.
                // Когда мы находим правильную букву, мы сохраняем ее индекс в 'letterToIndex'
                var letterToIndex = Array.FindIndex(AlphabetLetters, letter => letter == plaintextArray[i]);
                // Теперь добавляем к "старому" незашифрованному индексу сдвиг, который хотим использовать.
                // Итак, мы сдвигаем букву на количество, которое было объявлено в целочисленном сдвиге.
                letterToIndex += shiftCount;
                // Здесь мы используем новый найденный индекс буквы, по модулю на количество букв
                // (поэтому, если количество букв в алфавите было 26, а буква теперь 30, она вернется к началу, а не за пределы диапазона)
                // и сохранить новую букву в "зашифрованном" char[i], где i - правильный индекс размещения буквы в массиве.
                encrypted[i] = AlphabetLetters[letterToIndex % AlphabetLetters.Length];
            }

            // Здесь мы возвращаем только что зашифрованный текст
            return encrypted;
        }

        // Это функция шифрования ПЕРЕСТАНОВОЧНОГО ШИФРА
        private static char[] PermutationCipher(int[] permutationInts, char[] plaintextArray)
        {
            // Счетчик используется для определения перехода к следующему блоку шифра.
            // Например, если длина ключа равна 5 каждый раз, когда он достигает 6-го элемента (6-й 'i' равен 5), счетчик получит +1,
            // потому что мы ищем элементы, которые дадут в результате ноль.
            // Поскольку первый элемент также равен 0, я начинаю счетчик с -1, поэтому он станет равным 0, как только начнется цикл.
            int counter = -1;

            // Здесь мы объявляем новый массив символов.
            // Он будет содержать результат нашего зашифрованного открытого текста.
            // Длина "зашифрованного" символа такая же, как и у открытого массива, потому что он должен содержать такое же количество букв, но со смещением.
            char[] encrypted = new char[plaintextArray.Length];

            // Мы перебираем весь текст.
            // (Так что работаем с каждой буквой по отдельности).
            for (int i = 0; i < plaintextArray.Length; i++)
            {
                // Здесь мы проверяем, что буква из текста равна началу нового блока. Если это так, то счетчик получает +1.
                if (i % permutationInts.Length == 0) counter++;
                // location — это целое число, где текущая буква должна находиться в блоке.
                // например, если в перестановке int правило первой буквы стоит на 5-м месте,
                // тогда правая часть вернет 5 (по месту) -1 (потому что индексы в массиве считаются с нуля, а не с единицы).
                int location = (permutationInts[i % permutationInts.Length] - 1);

                // Здесь мы берем новое расположение буквы (местоположение) и добавляем индекс буквы в текст.
                // Это делается путем умножения текущего блока на количество букв в блоке.
                encrypted[i] = plaintextArray[location + counter * permutationInts.Length];
            }

            // Здесь мы возвращаем только что зашифрованный текст
            return encrypted;
        }
    }
} 