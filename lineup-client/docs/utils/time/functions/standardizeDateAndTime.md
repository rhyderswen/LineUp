[**lineup-client**](../../../index.md)

---

[lineup-client](../../../modules.md) / [utils/time](../index.md) / standardizeDateAndTime

# Function: standardizeDateAndTime()

> **standardizeDateAndTime**(`date`, `time`): `string`

Defined in: [utils/time.ts:157](https://github.com/rhyderswen/LineUp/blob/e2f17bae616784da1758a5d11711773573512737/lineup-client/src/utils/time.ts#L157)

Takes a Date and Time and converts it to a standardized ISO string in UTC.

## Parameters

### date

`Date`

### time

[`Time`](../../../types/type-aliases/Time.md)

## Returns

`string`

The ISO string, formatted "YYYY-MM-DDTHH:MM".

## Remarks

Calls [addTimeToDate](addTimeToDate.md), which sets the Date's time to the Time object.
