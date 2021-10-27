import { isDevelopment } from 'environment';

const localHostApiUrl = 'http://localhost:5005';
const developmentApiUrl = 'http://188.165.16.160:5005';

const apiUrl = isDevelopment ? developmentApiUrl : localHostApiUrl;

export const ORGANIZATION_URL = `${apiUrl}/Organization/`;
export const ACCOUNT_URL = `${apiUrl}/Account/`;
export const CATEGORIES_URL = `${apiUrl}/Category/`;
export const EXERCISES_URL = `${apiUrl}/Exercises`;
export const PLANS_URL = `${apiUrl}/Plans`;
export const PAYMENTS_URL = `${apiUrl}/Payments`;
export const USER_URL = `${apiUrl}/Users/`;
