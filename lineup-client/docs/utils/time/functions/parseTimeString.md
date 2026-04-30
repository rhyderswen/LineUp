[**lineup-client**](../../../index.md)

---

[lineup-client](../../../modules.md) / [utils/time](../index.md) / parseTimeString

# Function: parseTimeString()

> **parseTimeString**(`time`): [`Time`](../../../types/type-aliases/Time.md) \| `null`

Defined in: [utils/time.ts:78](https://github.com/rhyderswen/LineUp/blob/e2f17bae616784da1758a5d11711773573512737/lineup-client/src/utils/time.ts#L78)

Takes a time string in 24H format (e.g. `"23:59"`) and converts it to a [Time](../../../types/type-aliases/Time.md) object.

## Parameters

### time

`string`

## Returns

[`Time`](../../../types/type-aliases/Time.md) \| `null`

The converted Time object or `null` if the string is invalid.
