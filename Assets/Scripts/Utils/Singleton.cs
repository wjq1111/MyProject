public class Singleton<T> where T : class, new()
{
    private static T s_instance;
    public static T Instance
    {
        get
        {
            if (s_instance == null)
            {
                s_instance = CreateInstance();
            }
            return s_instance;
        }
    }

    public static T CreateInstance()
    {
        if (s_instance == null)
        {
            s_instance = new T();
            (s_instance as Singleton<T>).Init();
        }
        return s_instance;
    }

    public static void DestroyInstance()
    {
        if (s_instance != null)
        {
            (s_instance as Singleton<T>).Uninit();
            s_instance = null;
        }
    }



    public static bool HasInstance()
    {
        return s_instance != null;
    }

    public virtual void Init()
    {

    }

    public virtual void Uninit()
    {

    }
}
