
public static class IntExtension {

    /// <summary>
    /// 將數值修正到範圍內
    /// </summary>
    /// <param name="myInt"></param>
    public static void FixValueInRange(this ref int myInt, int minimum, int maximum) {
        if (maximum < minimum) {
            int newMax = minimum;
            int newMin = maximum;
            minimum = newMin;
            maximum = newMax;
        }
        if (myInt < minimum) {
            myInt = minimum;
        }
        if (myInt > maximum) {
            myInt = maximum;
        }
    }

}
