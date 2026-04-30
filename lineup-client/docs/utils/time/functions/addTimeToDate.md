[**lineup-client**](../../../index.md)

---

[lineup-client](../../../modules.md) / [utils/time](../index.md) / addTimeToDate

# Function: addTimeToDate()

> **addTimeToDate**(`date`, `time`): `Date`

Defined in: [utils/time.ts:147](https://github.com/rhyderswen/LineUp/blob/e2f17bae616784da1758a5d11711773573512737/lineup-client/src/utils/time.ts#L147)

Combines a Time object to a Date object, returning a new Date object with the specified day of the Date but the specified time of the Time object.

## Parameters

### date

`Date`

### time

[`Time`](../../../types/type-aliases/Time.md)

## Returns

`Date`

## Example

```ts
const date = const result = addTimeToDate(new Date("2024-01-01T10:15:00"), { hour: 9, minute: 30 });
// returns a Date object on January 1st at 9:30 AM.
```
