function validateOperands(a, b) {
    if (!Number.isFinite(a) || !Number.isFinite(b)) {
        return "Ошибка: нужно ввести два числа.";
    }
    if (a < 0 || b < 0) {
        return "Ошибка: отрицательные значения не допускаются.";
    }
    return null;
}

function add(a, b) {
    const error = validateOperands(a, b);
    if (error) return error;
    return a + b;
}

function subtract(a, b) {
    const error = validateOperands(a, b);
    if (error) return error;
    if (a < b) return "Ошибка: результат не может быть отрицательным.";
    return a - b;
}

function multiply(a, b) {
    const error = validateOperands(a, b);
    if (error) return error;
    return a * b;
}

function divide(a, b) {
    const error = validateOperands(a, b);
    if (error) return error;
    if (b === 0) return "Ошибка: делить на ноль нельзя.";
    return a / b;
}

module.exports = { add, subtract, multiply, divide };
