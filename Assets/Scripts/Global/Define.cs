
public static class Define {

#if UNITY_STANDALONE
    public static bool isPlatformPC = true;
#else
    public static bool isPlatformPC = false;
#endif

}
