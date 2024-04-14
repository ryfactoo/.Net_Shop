function getCartItems() {
    const cartItemsString = localStorage.getItem('cartItems');
    return cartItemsString ? JSON.parse(cartItemsString) : {};
}

function saveCartItems(cartItems) {
    localStorage.setItem('cartItems', JSON.stringify(cartItems));
}

function updateCartItemQuantity(itemId, newQuantity) {
    const cartItems = getCartItems();
    cartItems[itemId] = newQuantity;
    saveCartItems(cartItems);
}
function incrementCartItem(itemId) {
    const cartItems = getCartItems();
    const currentQuantity = cartItems[itemId] || 0;
    updateCartItemQuantity(itemId, currentQuantity + 1);
}

function decrementCartItem(itemId) {
    const cartItems = getCartItems();
    const currentQuantity = cartItems[itemId] || 0;
    updateCartItemQuantity(itemId, Math.max(currentQuantity - 1, 0));
}

function removeCartItem(itemId) {
    const cartItems = getCartItems();
    delete cartItems[itemId];
    saveCartItems(cartItems);
}