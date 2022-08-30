using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sparky;

public class BankAccount
{
	private int _balance;
	private ILogBook _logBook;
	public BankAccount(ILogBook logBook)
	{
        _balance = 0;
		_logBook = logBook;
	}
	public bool Deposit(int amount)
	{
		_logBook.Message("Deposit invoked");
        _logBook.Message("Test invoked");
		_logBook.LogSeverity = 101;
		var temp = _logBook.LogSeverity;

        _balance += amount;
		return true;
	}
    public bool Withraw(int amount)
    {
        if (amount <= _balance)
		{
            _logBook.LogToDb($"Withdrawal Amount: {amount}");
            _balance -= amount;
			return _logBook.LogBalanceAfterWithdrawal(_balance);
		}
        return _logBook.LogBalanceAfterWithdrawal(_balance - amount);
    }
	public int GetBalance() => _balance;

}
