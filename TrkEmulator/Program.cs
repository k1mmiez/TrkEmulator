namespace TrkEmulator
{
    static class Program
    {
        private static readonly List<string> _messages = new List<string>
        {
            @"#L#2.0;45637388;1234;5AE6\\r\\n",
            @"#D#050324;153057;NA;NA;NA;NA;0;0;0;0;0;NA;NA;;NA;tp:3:PG,sp0:1:0,sp1:1:10,r0:1:0,l0:2:7207.00,t0:2:0.00,d0:2:0.000,s0:2:0.00,lo0:1:0,r1:1:0,l1:2:8000.00,t1:2:0.00,d1:2:0.000,s1:2:0.00,lo1:1:0,r2:1:0,l2:2:36805.00,t2:2:0.00,d2:2:0.000,s2:2:0.00,lo2:1:0,r3:1:0,l3:2:119875.00,t3:2:0.00,d3:2:0.000,s3:2:0.00,lo3:1:0;80E6;\\r\\n",
            @"#D#070324;014953;NA;NA;NA;NA;0;0;0;0;0;NA;NA;;NA;tp:3:IF,vr:3:1.01,im:3:862057049103408,nk:3:862057049103408,fv:3:6.09,is:3:250021015840327,vh:3:KVOTA-3,vb:3:1.01,bh:3:f19c8f6;5E6E\\r\\n",
            @"#D#050324;161805;NA;NA;NA;NA;0;0;0;0;0;NA;NA;;NA;tp:3:TP,tb:1:0,pt:1:1024,lp:2:2000.45;FA8F\\r\\n",
            @"#D#050324;155018;NA;NA;NA;NA;0;0;0;0;0;NA;NA;;NA;tp:3:ET,rc:1:249,ds:3:050324,ts:3:155017,de:3:050324,se:3:155018,iu:1:18,nf:3:Система,fn:3:Вход в меню,nc:3:контроллера,nd:3:,no:1:0,nb:1:0,np:1:0,rf:2:0.00,ns:1:255,st:2:0.00,et:2:0.00,tt:2:0.0,dt:2:0.000;5E01\\r\\n",
            @"#D#050324;161805;NA;NA;NA;NA;0;0;0;0;0;NA;NA;;NA;tp:3:TE,tc:1:117,ds:3:050324,ts:3:161805,de:3:050324,se:3:161805,ty:1:0,ns:1:1,sl:2:8000.00,el:2:8500.00,sd:2:0.000,ed:2:0.000,sc:2:0,ec:2:0.0,ss:2:0.00,es:2:,si:1:0,vt:2:0.00,mt:2:,me:3:рандомная_инфа;ED\\r\\n"
        };
        
        static void Main()
        {
            TcpClientEmulator emulator = new TcpClientEmulator();
            Random random = new Random();

            while (emulator.IsConnected == false)
            {
                emulator.Start();
            }
            
            while (true)
            {
                try
                {
                    Thread.Sleep(random.Next(1000, 5000));
                    
                    emulator.SendMessage(_messages[random.Next(0, 6)]);
                    Console.WriteLine("Сообщение отправлено");

                    var answer = emulator.RecieveMessage();
                    Console.WriteLine($"Получено сообщение: {answer}");

                    if (string.IsNullOrEmpty(answer))
                    {
                        break;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    break;
                }
            }
            
            emulator.Stop();
        }
    }
}