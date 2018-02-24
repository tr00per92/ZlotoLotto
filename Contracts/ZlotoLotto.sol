pragma solidity ^0.4.18;

contract ZlotoLotto {
    
    address private owner = msg.sender;
    mapping(address => uint) private balances;
    
    uint public price;
    uint public currentSupply;
    
    modifier ownerOnly() {
        require(msg.sender == owner);
        _;
    }
    
    modifier requireBalance(uint balance) {
        require(balances[msg.sender] >= balance);
        _;
    }
    
    function ZlotoLotto(uint initialSupply, uint initialPriceInFinney) public payable {
        price = initialPriceInFinney * 1 finney;
        currentSupply = initialSupply;
        require(msg.value >= getMinimumBalance());
        balances[owner] = initialSupply;
    }
    
    function scratchToken() public requireBalance(1) returns(uint) {
        uint number = getRandom();
        if (number == 9) {
            checkForOverflow(balances[msg.sender], 3);
            checkForOverflow(currentSupply, 3);
            balances[msg.sender] += 3;
            currentSupply += 3;
            return 4;
        }
        
        if (number == 8) {
            checkForOverflow(balances[msg.sender], 1);
            checkForOverflow(currentSupply, 1);
            balances[msg.sender]++;
            currentSupply++;
            return 2;   
        } 
        
        if (number >= 5) {
            return 1;
        } 
        
        balances[msg.sender]--;
        currentSupply--;
        return 0;
    }
    
    function buyTokens() public payable {
        buyTokensPrivate(msg.value / price);
    }
    
    function buyTokens(uint amount) public payable {
        buyTokensPrivate(amount);
    }
    
    function sellTokens(uint amount) public requireBalance(amount) {
        uint value = amount * price;
        require(this.balance >= value);
        checkForOverflow(balances[owner], amount);
        
        balances[msg.sender] -= amount;
        balances[owner] += amount;
        msg.sender.transfer(value);
    }
    
    function getTokensCount() public view returns(uint) {
        return balances[msg.sender];
    }
    
    function getBalance() public view ownerOnly returns(uint) {
        return this.balance;
    }
    
    function getMinimumBalance() public view ownerOnly returns(uint) {
        return currentSupply * price / 200;
    }
    
    function deposit() public payable ownerOnly { }
    
    function withdraw(uint value) public ownerOnly {
        require(this.balance - value >= getMinimumBalance());
        owner.transfer(value);
    }
    
    function () public payable { }
    
    function getRandom() private view returns(uint) {
        // returns a number between 0 and 9
        return uint(block.blockhash(block.number - 1)) % 10;
    }
    
    function buyTokensPrivate(uint amount) private {
        require(balances[owner] >= amount);
        checkForOverflow(balances[msg.sender], amount);
        require(price * amount == msg.value);
        balances[owner] -= amount;
        balances[msg.sender] += amount;
    }
    
    function checkForOverflow(uint value, uint amount) private {
        require(value + amount >= value);
    }
}
