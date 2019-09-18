namespace CC.Connections.BL
{
    public class Member_Type
    {
        private int? memeberTypeID;

        public Member_Type(int? memeberTypeID)
        {
            this.memeberTypeID = memeberTypeID;
        }

        public int ID { get; set; }
        public string Desc { get; set; }
    }
}