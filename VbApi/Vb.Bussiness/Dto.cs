// CustomerDTO
namespace Vb.Business;
public class CustomerDto
{
    public string IdentityNumber { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int CustomerNumber { get; set; }
    public DateTime DateOfBirth { get; set; }
    public DateTime LastActivityDate { get; set; }

    public List<AddressDto> Addresses { get; set; }
    public List<ContactDto> Contacts { get; set; }
    public List<AccountDto> Accounts { get; set; }
}

// AddressDTO
public class AddressDto
{
    public string Address1 { get; set; }
    public string Address2 { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
    public string County { get; set; }
    public string PostalCode { get; set; }
    public bool IsDefault { get; set; }
}

// ContactDTO
public class ContactDto
{
    public string ContactType { get; set; }
    public string Information { get; set; }
    public bool IsDefault { get; set; }
}

// AccountDTO
public class AccountDto
{
    public int AccountNumber { get; set; }
    public string IBAN { get; set; }
    public decimal Balance { get; set; }
    public string CurrencyType { get; set; }
    public string Name { get; set; }
    public DateTime OpenDate { get; set; }

    public List<AccountTransactionDto> AccountTransactions { get; set; }
    public List<EftTransactionDto> EftTransactions { get; set; }
}

// AccountTransactionDTO
public class AccountTransactionDto
{
    public string ReferenceNumber { get; set; }
    public DateTime TransactionDate { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; }
    public string TransferType { get; set; }
}

// EftTransactionDTO
public class EftTransactionDto
{
    public string ReferenceNumber { get; set; }
    public DateTime TransactionDate { get; set; }
    public decimal? Amount { get; set; }
    public string Description { get; set; }
    public string SenderAccount { get; set; }
    public string SenderIban { get; set; }
    public string SenderName { get; set; }
}
