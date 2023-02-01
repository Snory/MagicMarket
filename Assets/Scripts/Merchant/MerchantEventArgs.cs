using System;

public class MerchantEventArgs : EventArgs
{
    public Merchant Merchant;

    public MerchantEventArgs(Merchant merchant)
    {
        this.Merchant = merchant;
    }
}