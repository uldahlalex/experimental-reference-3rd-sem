import http from 'k6/http';
import { sleep } from 'k6';
export const options = {
  vus: 100,
  duration: '5s',
};
export default function () {
  http.get('http://localhost:5000/api/discover');
}