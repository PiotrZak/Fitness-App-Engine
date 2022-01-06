import config from './../../config.json';

const bucketName = 'planfi-movies'
const fileBucketName = 'planfi-files'

export const movieUrl = `https://storage.cloud.google.com/${bucketName}`
export const imageUrl = `https://storage.cloud.google.com/${fileBucketName}`
const localHostApiUrl = 'http://localhost:9001'
const developmentApiUrl = config.apps.PlanfiApi.url

export const apiUrl = config.apps.PlanfiApi.isProduction
  ? developmentApiUrl
  : localHostApiUrl

export const ORGANIZATION_URL = `${apiUrl}/Organization/`
export const ACCOUNT_URL = `${apiUrl}/Account/`
export const CATEGORIES_URL = `${apiUrl}/Category/`
export const EXERCISES_URL = `${apiUrl}/Exercises`
export const PLANS_URL = `${apiUrl}/Plans`
export const PAYMENTS_URL = `${apiUrl}/Payments`
export const USER_URL = `${apiUrl}/Users/`

export const CHAT_URL = `${apiUrl}/chat/`
export const MESSAGE_URL = `${apiUrl}/message/`
